import { useEffect, useState } from "react";
import axios from "axios";
import MenuFijo from "@/components/MenuFijo";

function MensajesRecibidosScout() {
  const [mensajes, setMensajes] = useState([]);
  const token = localStorage.getItem("token");

  useEffect(() => {
    const cargarMensajes = async () => {
      try {
        const resUsuario = await axios.get("http://localhost:8080/api/users/me", {
          headers: { Authorization: `Bearer ${token}` },
        });

        const unidadId = resUsuario.data.unidad?.id;
        if (!unidadId) {
          alert("No se encontró tu unidad.");
          return;
        }

        const resMensajes = await axios.get(
          `http://localhost:8080/api/mensajes/por-unidad?unidadId=${unidadId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );

        setMensajes(resMensajes.data);
      } catch (error) {
        console.error("Error al cargar los mensajes:", error);
        alert("No se pudieron cargar los mensajes recibidos.");
      }
    };

    cargarMensajes();
  }, [token]);

  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo superior (escritorio) */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      {/* Contenido */}
      <div className="max-w-2xl mx-auto pt-6 px-4">
        <div className="bg-white p-6 rounded shadow">
          <h2 className="text-2xl font-bold mb-4">📩 Mensajes de tu Unidad</h2>

          {mensajes.length === 0 ? (
            <p>No tienes mensajes recientes.</p>
          ) : (
            <ul className="space-y-4">
              {mensajes.map((mensaje) => (
                <li key={mensaje.id} className="border p-4 rounded bg-gray-50">
                  <p className="text-sm text-gray-500 mb-1">
                    📤 Enviado por: <strong>{mensaje.nombreDirigente || "(sin nombre)"}</strong> el{" "}
                    {new Date(mensaje.fecha).toLocaleString()}
                  </p>
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
      </div>

      {/* Menú fijo inferior (móvil) */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}

export default MensajesRecibidosScout;
