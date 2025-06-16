import { useEffect, useState } from "react";
import axios from "axios";
import ModalCodigoReset from "../components/ModalCodigoReset";
import MenuFijo from "@/components/MenuFijo";

function MiembrosUnidad() {
  const [miembros, setMiembros] = useState([]);
  const token = localStorage.getItem("token");
  const [dirigenteId, setDirigenteId] = useState(null);
  const [codigoGenerado, setCodigoGenerado] = useState(null);
  const [nombreUsuarioCodigo, setNombreUsuarioCodigo] = useState("");

  useEffect(() => {
    cargarMiembros();
  }, []);

  const cargarMiembros = async () => {
    try {
      const response = await axios.get("http://localhost:8080/api/users/me", {
        headers: { Authorization: `Bearer ${token}` },
      });

      const id = response.data.id;
      setDirigenteId(id);

      const resMiembros = await axios.get(
        `http://localhost:8080/api/users/miembros-unidad/${id}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );

      setMiembros(resMiembros.data);
    } catch (error) {
      console.error("Error al cargar miembros de la unidad", error);
    }
  };

  const eliminarMiembro = async (usuarioId) => {
    const confirmacion = confirm("¿Estás seguro de eliminar a este miembro de la unidad?");
    if (!confirmacion) return;

    try {
      await axios.post(
        "http://localhost:8080/api/users/eliminar-de-unidad",
        { dirigenteId, usuarioId },
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      cargarMiembros();
    } catch (error) {
      console.error("Error al eliminar miembro", error);
    }
  };

  const generarCodigo = async (usuarioId, nombreCompleto) => {
    try {
      const res = await axios.post(
        `http://localhost:8080/api/users/${dirigenteId}/generar-codigo-reset/${usuarioId}`,
        {},
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setCodigoGenerado(res.data.codigo);
      setNombreUsuarioCodigo(nombreCompleto);
    } catch (error) {
      console.error("Error al generar código", error);
      alert("No se pudo generar el código.");
    }
  };

  const cerrarModal = () => {
    setCodigoGenerado(null);
    setNombreUsuarioCodigo("");
  };

  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo superior en escritorio */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      {/* Contenido principal */}
      <div className="max-w-3xl mx-auto pt-6 px-4">
        <div className="bg-white p-6 rounded shadow mt-4">
          <h1 className="text-2xl font-bold mb-4">👥 Miembros de la Unidad</h1>
          {miembros.length === 0 ? (
            <p>No hay miembros registrados en la unidad.</p>
          ) : (
            <ul className="divide-y divide-gray-200">
              {miembros.map((miembro) => (
                <li key={miembro.id} className="py-3 flex justify-between items-center">
                  <div>
                    <p className="font-medium">{miembro.nombreCompleto}</p>
                    <p className="text-sm text-gray-500">{miembro.tipo} – {miembro.rama}</p>
                  </div>
                  <div className="flex gap-2">
                    <button
                      onClick={() => generarCodigo(miembro.id, miembro.nombreCompleto)}
                      className="bg-blue-600 text-white px-3 py-1 rounded hover:bg-blue-700 text-sm"
                    >
                      Restablecer contraseña
                    </button>
                    <button
                      onClick={() => eliminarMiembro(miembro.id)}
                      className="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600 text-sm"
                    >
                      Retirar
                    </button>
                  </div>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>

      {/* Menú fijo inferior en móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      {/* Modal de código generado */}
      {codigoGenerado && (
        <ModalCodigoReset
          codigo={codigoGenerado}
          nombreUsuario={nombreUsuarioCodigo}
          onClose={cerrarModal}
        />
      )}
    </div>
  );
}

export default MiembrosUnidad;
