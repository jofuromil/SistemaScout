import React, { useEffect, useState } from "react";
import axios from "axios";

const ObjetivosSeleccionados = () => {
  const [pendientes, setPendientes] = useState([]);
  const [aprobados, setAprobados] = useState([]);
  const [error, setError] = useState(null);
  const [cargando, setCargando] = useState(true);

  const token = localStorage.getItem("token");
  const usuarioId = localStorage.getItem("usuarioId"); // 👈 se recupera desde el localStorage

  useEffect(() => {
    const fetchData = async () => {
      try {
        console.log("🔥 ESTE ES EL NUEVO COMPONENTE");
        const config = {
          headers: { Authorization: `Bearer ${token}` },
        };

        const resPendientes = await axios.get(`/api/Objetivo/pendientes-scout?usuarioId=${usuarioId}`, config);
        const resAprobados = await axios.get(`/api/Objetivo/historial?usuarioId=${usuarioId}&soloValidados=true`, config);

        console.log("📦 Pendientes crudos:", resPendientes.data);
        console.log("📦 Aprobados crudos:", resAprobados.data);

        const pendientesData = Array.isArray(resPendientes.data)
          ? resPendientes.data
          : Array.isArray(resPendientes.data.objetivos)
          ? resPendientes.data.objetivos
          : [];

        const aprobadosData = Array.isArray(resAprobados.data)
          ? resAprobados.data
          : Array.isArray(resAprobados.data.objetivos)
          ? resAprobados.data.objetivos
          : [];

        setPendientes(pendientesData);
        setAprobados(aprobadosData);
      } catch (err) {
        console.error("❌ Error de carga:", err);
        setError(err.message || "Error desconocido");
      } finally {
        setCargando(false);
      }
    };

    fetchData();
  }, [token, usuarioId]);

  if (cargando) return <div className="p-4">⏳ Cargando objetivos...</div>;
  if (error) return <div className="p-4 text-red-600 font-bold">❌ Error: {error}</div>;

  return (
    <div className="space-y-8 p-4">
      <section>
        <h2 className="text-xl font-bold text-yellow-600 mb-3">🕒 Pendientes de Aprobación</h2>
        {Array.isArray(pendientes) && pendientes.length > 0 ? (
          <ul className="space-y-2">
            {pendientes.map((obj, i) => (
              <li key={i} className="bg-yellow-100 p-3 rounded shadow">
                <strong>{obj.objetivoEducativo?.area || "Área no definida"}</strong>: {obj.objetivoEducativo?.descripcion || "Sin descripción"}
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-500">No tienes objetivos pendientes.</p>
        )}
      </section>

      <section>
        <h2 className="text-xl font-bold text-green-700 mb-3">✅ Aprobados</h2>
        {Array.isArray(aprobados) && aprobados.length > 0 ? (
          <ul className="space-y-2">
            {aprobados.map((obj, i) => (
              <li key={i} className="bg-green-100 p-3 rounded shadow">
                <strong>{obj.objetivoEducativo?.area || "Área no definida"}</strong>: {obj.objetivoEducativo?.descripcion || "Sin descripción"}
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-500">No tienes objetivos aprobados aún.</p>
        )}
      </section>
    </div>
  );
};

export default ObjetivosSeleccionados;
