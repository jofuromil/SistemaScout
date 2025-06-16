import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import MenuFijo from "@/components/MenuFijo";

const EventoDetalleDirigente = () => {
  const { eventoId } = useParams();

  const [evento, setEvento] = useState(null);
  const [inscritos, setInscritos] = useState([]);
  const [loadingEvento, setLoadingEvento] = useState(true);
  const [loadingInscritos, setLoadingInscritos] = useState(true);

  useEffect(() => {
    const fetchDatos = async () => {
      const token = localStorage.getItem("token");
      const dirigenteId = localStorage.getItem("usuarioId");

      try {
        const eventoRes = await axios.get(`/api/Evento/${eventoId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setEvento(eventoRes.data);
        setLoadingEvento(false);
      } catch (error) {
        console.error("❌ Error al cargar el evento:", error);
        alert("No se pudieron cargar los datos del evento.");
        setLoadingEvento(false);
        return;
      }

      try {
        const inscritosRes = await axios.get(
          `/api/Evento/${eventoId}/inscritos/${dirigenteId}`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        setInscritos(inscritosRes.data);
      } catch (error) {
        console.error("⚠️ Error al cargar los inscritos:", error);
        alert("No se pudieron cargar los inscritos del evento.");
      } finally {
        setLoadingInscritos(false);
      }
    };

    fetchDatos();
  }, [eventoId]);

  const cambiarEstado = async (usuarioId, nuevoEstado) => {
    const token = localStorage.getItem("token");
    const dirigenteId = localStorage.getItem("usuarioId");

    try {
      const res = await axios.post(
        "/api/Evento/actualizar-estado",
        {
          eventoId,
          usuarioId,
          dirigenteId,
          nuevoEstado,
        },
        { headers: { Authorization: `Bearer ${token}` } }
      );

      alert(res.data.mensaje);
      setInscritos((prev) =>
        prev.map((i) =>
          i.id === usuarioId ? { ...i, estado: nuevoEstado } : i
        )
      );
    } catch (error) {
      console.error("Error al cambiar estado:", error);
      alert("No se pudo cambiar el estado de inscripción.");
    }
  };

  return (
    <div className="min-h-screen bg-white text-gray-800 pb-20">
      {/* Menú superior fijo para pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-4xl mx-auto mt-6 px-4">
        {loadingEvento ? (
          <p>Cargando evento...</p>
        ) : !evento ? (
          <p>Error al cargar el evento.</p>
        ) : (
          <div>
            <h2 className="text-3xl font-bold mb-2">{evento.nombre}</h2>
            <p className="mb-4 text-gray-600">{evento.descripcion}</p>
            <p>
              <strong>Fecha:</strong>{" "}
              {new Date(evento.fechaInicio).toLocaleDateString()} -{" "}
              {new Date(evento.fechaFin).toLocaleDateString()}
            </p>
            <p>
              <strong>Nivel:</strong> {evento.nivel}
            </p>
            <p>
              <strong>Cupo Máximo:</strong> {evento.cupoMaximo || "Sin límite"}
            </p>

            <h3 className="text-2xl font-semibold mt-6 mb-2">Inscritos</h3>
            {loadingInscritos ? (
              <p>Cargando inscritos...</p>
            ) : inscritos.length === 0 ? (
              <p>No hay inscritos en este evento.</p>
            ) : (
              <table className="min-w-full bg-white rounded shadow text-sm">
                <thead className="bg-gray-200">
                  <tr>
                    <th className="px-4 py-2 text-left">Nombre</th>
                    <th className="px-4 py-2 text-left">Correo</th>
                    <th className="px-4 py-2 text-left">Estado</th>
                    <th className="px-4 py-2 text-left">Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {inscritos.map((i) => (
                    <tr key={i.id} className="border-t">
                      <td className="px-4 py-2">{i.nombreCompleto}</td>
                      <td className="px-4 py-2">{i.correo}</td>
                      <td className="px-4 py-2">{i.estado}</td>
                      <td className="px-4 py-2">
                        {i.estado === "Pendiente" ? (
                          <>
                            <button
                              onClick={() => cambiarEstado(i.id, "Aceptado")}
                              className="bg-green-500 hover:bg-green-600 text-white px-2 py-1 rounded mr-2"
                            >
                              ✅ Aceptar
                            </button>
                            <button
                              onClick={() => cambiarEstado(i.id, "Rechazado")}
                              className="bg-red-500 hover:bg-red-600 text-white px-2 py-1 rounded"
                            >
                              ❌ Rechazar
                            </button>
                          </>
                        ) : (
                          <span className="text-gray-500 italic">
                            Sin acciones
                          </span>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        )}
      </div>

      {/* Menú inferior fijo para móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
};

export default EventoDetalleDirigente;
