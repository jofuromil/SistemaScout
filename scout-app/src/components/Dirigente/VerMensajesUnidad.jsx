import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function VerMensajesUnidad() {
  const [mensajes, setMensajes] = useState([]);
  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  useEffect(() => {
    const cargarMensajes = async () => {
      try {
        const resUsuario = await axios.get("http://localhost:8080/api/users/me", {
          headers: { Authorization: `Bearer ${token}` },
        });

        const unidadId = resUsuario.data.unidad?.id;
        if (!unidadId) {
          alert("No se encontró la unidad del dirigente.");
          return;
        }

        const resMensajes = await axios.get(
          `http://localhost:8080/api/mensajes/por-unidad?unidadId=${unidadId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );

        setMensajes(resMensajes.data);
      } catch (error) {
        console.error("Error al cargar los mensajes:", error);
        alert("No se pudieron cargar los mensajes de la unidad.");
      }
    };

    cargarMensajes();
  }, [token]);

  const eliminarMensaje = async (id) => {
    const confirmar = confirm("¿Estás seguro de eliminar este mensaje?");
    if (!confirmar) return;

    try {
      await axios.delete(`http://localhost:8080/api/mensajes/eliminar/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setMensajes((prev) => prev.filter((m) => m.id !== id));
    } catch (error) {
      console.error("Error al eliminar el mensaje:", error);
      alert("No se pudo eliminar el mensaje.");
    }
  };

  return (
    <div className="max-w-2xl mx-auto mt-10 bg-white p-6 rounded shadow">
      <h2 className="text-2xl font-bold mb-4">📬 Mensajes enviados a la unidad</h2>

      <div className="mb-6 space-x-2">
        <button onClick={() => navigate("/panel-dirigente")} className="bg-gray-600 text-white px-3 py-2 rounded">
          🔙 Volver al Panel
        </button>
        <button onClick={() => navigate("/mensajes-unidad")} className="bg-blue-600 text-white px-3 py-2 rounded">
          ➕ Enviar nuevo mensaje
        </button>
      </div>

      {mensajes.length === 0 ? (
        <p>No hay mensajes registrados.</p>
      ) : (
        <ul className="space-y-4">
          {mensajes.map((mensaje) => (
            <li key={mensaje.id} className="border p-4 rounded bg-gray-50">
              <div className="flex justify-between items-center">
                <p className="text-sm text-gray-500 mb-1">
                  🤓 Enviado por: <strong>{mensaje.dirigente?.nombreCompleto || mensaje.nombreDirigente || "(sin nombre)"}</strong> el {new Date(mensaje.fecha).toLocaleString()}
                </p>
                <button
                  onClick={() => eliminarMensaje(mensaje.id)}
                  className="text-red-500 text-sm hover:underline"
                >
                  🗑️ Eliminar
                </button>
              </div>
              <p className="mb-2">📄 {mensaje.contenido}</p>

              {mensaje.rutaImagen && (
                <div className="mt-2">
                  <img
                    src={`http://localhost:8080/${mensaje.rutaImagen.replace(/\\/g, "/")}`}
                    alt="Imagen adjunta"
                    className="max-w-full border rounded shadow"
                  />
                </div>
              )}

              {mensaje.rutaArchivo && (
                <div className="mt-2">
                  <a
                    href={`http://localhost:8080/${mensaje.rutaArchivo.replace(/\\/g, "/")}`}
                    target="_blank"
                    rel="noopener noreferrer"
                    className="text-blue-600 underline"
                  >
                    📎 Descargar archivo adjunto
                  </a>
                </div>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default VerMensajesUnidad;
