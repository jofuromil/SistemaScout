import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

function Unidad() {
  const token = localStorage.getItem("token");
  const usuarioId = localStorage.getItem("usuarioId");
  const tipo = localStorage.getItem("tipo") || "";
  const navigate = useNavigate();

  const [ramaUnirse, setRamaUnirse] = useState("");
  const [codigoUnidad, setCodigoUnidad] = useState("");
  const [nombre, setNombre] = useState("");
  const [rama, setRama] = useState("");
  const [distrito, setDistrito] = useState("");
  const [grupo, setGrupo] = useState("");
  const [gruposPorDistrito, setGruposPorDistrito] = useState({});
  const [mensaje, setMensaje] = useState("");
  const [codigoCreado, setCodigoCreado] = useState("");

  useEffect(() => {
    if (!token || !usuarioId) {
      navigate("/login");
      return;
    }

    axios
      .get("http://localhost:8080/api/unidades/grupos-por-distrito")
      .then((res) => setGruposPorDistrito(res.data))
      .catch(() => setMensaje("❌ Error al cargar grupos por distrito."));
  }, []);

  const distritos = Object.keys(gruposPorDistrito);
  const gruposDisponibles = gruposPorDistrito[distrito] || [];

  const handleUnirse = async (e) => {
    e.preventDefault();
    setMensaje("");

    try {
      const res = await axios.post(
        "http://localhost:8080/api/users/unirse-a-unidad",
        {
          usuarioId,
          codigoUnidad,
          rama: ramaUnirse,
        },
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      const usuario = res.data;
      localStorage.setItem("usuario", JSON.stringify(usuario));
      localStorage.setItem("unidadCodigo", usuario.unidadCodigo || "");
      localStorage.setItem("unidadNombre", usuario.unidadNombre || "");
      localStorage.setItem("unidadRama", usuario.unidadRama || "");

      const tipoUsuario = tipo.toLowerCase();
      if (tipoUsuario === "dirigente") {
        navigate("/panel-dirigente");
      } else {
        navigate("/panel-scout");
      }
    } catch (error) {
      const msg = error?.response?.data?.mensaje || "Error al unirse.";
      setMensaje(`❌ ${msg}`);
    }
  };

  const handleCrear = async (e) => {
    e.preventDefault();
    setMensaje("");
    setCodigoCreado("");

    if (tipo.toLowerCase() !== "dirigente") {
      setMensaje("❌ Solo los dirigentes pueden crear unidades.");
      return;
    }

    try {
      const res = await axios.post(
        "http://localhost:8080/api/unidades/crear",
        {
          nombre,
          rama,
          distrito,
          grupoScout: grupo,
          dirigenteId: usuarioId,
        },
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      const unidad = res.data;
      localStorage.setItem("unidadCodigo", unidad.codigo || "");
      localStorage.setItem("unidadNombre", unidad.nombre || "");
      localStorage.setItem("unidadRama", unidad.rama || "");
      localStorage.setItem("usuario", JSON.stringify({
        nombreCompleto: localStorage.getItem("nombreCompleto") || "",
        unidadNombre: unidad.nombre,
        unidadCodigo: unidad.codigo,
        unidadRama: unidad.rama,
        rol: "Dirigente",
        token
      }));

      setCodigoCreado(unidad.codigo || unidad.id || "");
      setMensaje("✅ Unidad creada con éxito.");
      setTimeout(() => navigate("/panel-dirigente"), 2000);
    } catch (error) {
      const msg = error?.response?.data?.mensaje || "Error al crear unidad.";
      setMensaje(`❌ ${msg}`);
    }
  };

  return (
    <div className="min-h-screen bg-purple-600 text-white flex flex-col justify-center items-center px-4 py-8">
      <h2 className="text-3xl font-bold text-center mb-6">Gestión de Unidad</h2>

      {mensaje && <p className="text-center mb-4 text-red-200">{mensaje}</p>}
      {codigoCreado && (
        <p className="text-center mb-4 text-green-200 font-bold">
          📌 Código de unidad: {codigoCreado}
        </p>
      )}

      <div className="w-full max-w-md mb-10">
        <h3 className="text-lg font-semibold mb-2">Unirse a una unidad existente</h3>
        <form onSubmit={handleUnirse} className="flex flex-col gap-3">
          <select
            value={ramaUnirse}
            onChange={(e) => setRamaUnirse(e.target.value)}
            required
            className="p-3 rounded-lg bg-white text-black"
          >
            <option value="">Selecciona una rama</option>
            <option>Lobatos</option>
            <option>Exploradores</option>
            <option>Pioneros</option>
            <option>Rovers</option>
          </select>

          <input
            type="text"
            value={codigoUnidad}
            onChange={(e) => setCodigoUnidad(e.target.value)}
            placeholder="Código de la unidad"
            required
            className="p-3 rounded-lg bg-white text-black placeholder-gray-500"
          />

          <button
            type="submit"
            className="flex items-center justify-center gap-2 border-2 border-white text-white py-3 rounded-full font-semibold hover:bg-white hover:text-purple-700 transition"
          >
            Unirme
          </button>
        </form>
      </div>

      {tipo.toLowerCase() === "dirigente" && (
        <>
          <div className="text-center font-bold my-4">o</div>

          <div className="w-full max-w-md">
            <h3 className="text-lg font-semibold mb-2">Crear nueva unidad (solo dirigentes)</h3>
            <form onSubmit={handleCrear} className="flex flex-col gap-3">
              <input
                type="text"
                value={nombre}
                onChange={(e) => setNombre(e.target.value)}
                placeholder="Nombre de la unidad"
                required
                className="p-3 rounded-lg bg-white text-black placeholder-gray-500"
              />

              <select
                value={rama}
                onChange={(e) => setRama(e.target.value)}
                required
                className="p-3 rounded-lg bg-white text-black"
              >
                <option value="">Selecciona una rama</option>
                <option>Lobatos</option>
                <option>Exploradores</option>
                <option>Pioneros</option>
                <option>Rovers</option>
              </select>

              <select
                value={distrito}
                onChange={(e) => setDistrito(e.target.value)}
                required
                className="p-3 rounded-lg bg-white text-black"
              >
                <option value="">Selecciona un distrito</option>
                {distritos.map((d) => (
                  <option key={d}>{d}</option>
                ))}
              </select>

              <select
                value={grupo}
                onChange={(e) => setGrupo(e.target.value)}
                required
                className="p-3 rounded-lg bg-white text-black"
              >
                <option value="">Selecciona un grupo</option>
                {gruposDisponibles.map((g) => (
                  <option key={g}>{g}</option>
                ))}
              </select>

              <button
                type="submit"
                className="flex items-center justify-center gap-2 border-2 border-white text-white py-3 rounded-full font-semibold hover:bg-white hover:text-purple-700 transition"
              >
                Crear Unidad
              </button>
            </form>
          </div>
        </>
      )}
    </div>
  );
}

export default Unidad;
