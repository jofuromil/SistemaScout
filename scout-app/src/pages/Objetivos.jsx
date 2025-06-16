import { useEffect, useState } from "react";
import axios from "axios";
import MenuFijo from "@/components/MenuFijo";

function Objetivos() {
  const token = localStorage.getItem("token");
  const usuarioId = localStorage.getItem("usuarioId");
  const rama = localStorage.getItem("rama");
  const nivelProgresion = localStorage.getItem("nivelProgresion") || "";

  const [objetivos, setObjetivos] = useState([]);
  const [seleccionados, setSeleccionados] = useState([]);
  const [mensaje, setMensaje] = useState("");

  useEffect(() => {
    if (!token || !usuarioId || !rama) {
      window.location.href = "/login";
      return;
    }

    axios
      .get(`http://localhost:8080/api/Objetivo/historial?usuarioId=${usuarioId}`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        const ids = res.data.map((o) => o.id);
        setSeleccionados(ids);
      })
      .catch(() => setMensaje("❌ Error al cargar historial de objetivos."));
  }, [usuarioId, token]);

  useEffect(() => {
    if (!token || !rama) return;

    let url = `http://localhost:8080/api/Objetivo/listar?rama=${encodeURIComponent(rama)}`;
    if (nivelProgresion) {
      url += `&nivelProgresion=${encodeURIComponent(nivelProgresion)}`;
    }

    axios
      .get(url, { headers: { Authorization: `Bearer ${token}` } })
      .then((res) => setObjetivos(res.data))
      .catch(() => setMensaje("❌ Error al cargar los objetivos."));
  }, [rama, nivelProgresion, token]);

  const seleccionar = (id) => {
    axios
      .post(
        "http://localhost:8080/api/Objetivo/seleccionar",
        { objetivoEducativoId: id },
        { headers: { Authorization: `Bearer ${token}` } }
      )
      .then(() => {
        setSeleccionados((prev) => [...prev, id]);
        setMensaje("✅ Objetivo seleccionado correctamente.");
      })
      .catch(() => setMensaje("❌ Hubo un problema al seleccionar el objetivo."));
  };

  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo superior en pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      {/* Contenido principal */}
      <div className="max-w-3xl mx-auto pt-6 px-4">
        <h1 className="text-2xl font-bold mb-4">🎯 Seleccionar Objetivos Educativos</h1>
        {mensaje && <p className="mb-4 text-sm text-center">{mensaje}</p>}
        {objetivos.length === 0 ? (
          <p>No hay objetivos disponibles para esta selección.</p>
        ) : (
          objetivos.map((obj) => (
            <div
              key={obj.id}
              className={`bg-white p-4 mb-4 rounded shadow transition ${
                seleccionados.includes(obj.id) ? "bg-green-100" : ""
              }`}
            >
              <h3 className="text-lg font-semibold">{obj.nombre}</h3>
              <p><strong>Área:</strong> {obj.area}</p>
              <p className="mb-2">{obj.descripcion}</p>
              <button
                onClick={() => seleccionar(obj.id)}
                disabled={seleccionados.includes(obj.id)}
                className={`py-2 px-4 rounded ${
                  seleccionados.includes(obj.id)
                    ? "bg-gray-400 cursor-not-allowed"
                    : "bg-green-600 hover:bg-green-700 text-white"
                }`}
              >
                {seleccionados.includes(obj.id) ? "Seleccionado" : "Seleccionar"}
              </button>
            </div>
          ))
        )}
      </div>

      {/* Menú fijo inferior en móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}

export default Objetivos;
