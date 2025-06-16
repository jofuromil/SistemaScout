import { useEffect, useState } from "react";
import axios from "axios";
import MenuFijo from "@/components/MenuFijo";

function EventosScout() {
  const [eventos, setEventos] = useState([]);
  const [eventosInscritos, setEventosInscritos] = useState([]);
  const [mensaje, setMensaje] = useState("");

  const token = localStorage.getItem("token");
  const usuarioId = localStorage.getItem("usuarioId");

  useEffect(() => {
    const cargarEventos = async () => {
      if (!token || !usuarioId) {
        setMensaje("Error: Usuario no autenticado.");
        return;
      }

      try {
        const userRes = await axios.get("/api/users/me", {
          headers: { Authorization: `Bearer ${token}` },
        });

        const codigoUnidad = userRes.data?.unidad?.codigoUnidad;
        if (!codigoUnidad) {
          setMensaje("No estás vinculado a una unidad.");
          return;
        }

        const eventosRes = await axios.get(`/api/evento/unidad/${codigoUnidad}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setEventos(eventosRes.data);

        const inscritosRes = await axios.get(`/api/evento/mis-eventos/${usuarioId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        const idsInscritos = inscritosRes.data.map((e) => e.id);
        setEventosInscritos(idsInscritos);
      } catch (error) {
        console.error("Error al cargar eventos:", error);
        setMensaje("No se pudieron cargar los eventos.");
      }
    };

    cargarEventos();
  }, [token, usuarioId]);

  const inscribirse = async (eventoId) => {
    try {
      const res = await axios.post(
        "/api/evento/inscribirse",
        {
          EventoId: eventoId,
          UsuarioId: usuarioId,
          TipoParticipacion: "Participante",
        },
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );

      setMensaje(res.data?.mensaje || "Inscripción enviada correctamente.");
      setEventosInscritos((prev) => [...prev, eventoId]);
    } catch (err) {
      console.error("Error al inscribirse:", err);
      const msg = err.response?.data?.mensaje || "Error al intentar inscribirse.";
      setMensaje("Error: " + msg);
    }
  };

  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo superior en pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-3xl mx-auto pt-6 px-4">
        <h1 className="text-2xl font-bold mb-4">📅 Eventos de tu Unidad</h1>

        {mensaje && (
          <div className="mb-4 p-3 bg-blue-100 text-blue-800 rounded">
            {mensaje}
          </div>
        )}

        {eventos.length === 0 ? (
          <p>No hay eventos disponibles.</p>
        ) : (
          eventos.map((evento) => (
            <div key={evento.id} className="border p-4 rounded mb-4 bg-gray-50">
              <h2 className="text-xl font-semibold">{evento.nombre}</h2>
              <p className="text-sm text-gray-600">
                📍 {evento.lugar} | 🗓️{" "}
                {new Date(evento.fechaInicio).toLocaleDateString()} al{" "}
                {new Date(evento.fechaFin).toLocaleDateString()}
              </p>
              <p className="mt-2">{evento.descripcion}</p>

              {eventosInscritos.includes(evento.id) ? (
                <button
                  disabled
                  className="mt-3 bg-gray-400 text-white px-4 py-2 rounded cursor-not-allowed"
                >
                  ✅ Ya inscrito
                </button>
              ) : (
                <button
                  onClick={() => inscribirse(evento.id)}
                  className="mt-3 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
                >
                  Inscribirme
                </button>
              )}
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

export default EventosScout;
