import React, { useEffect, useState } from "react";
import axios from "axios";
import MenuFijo from "@/components/MenuFijo";

export default function ResumenEspecialidadesDirigente() {
  const [resumen, setResumen] = useState([]);
  const token = localStorage.getItem("token");
  const unidadId = localStorage.getItem("unidadId");

  useEffect(() => {
    axios
      .get(`/api/especialidades/resumen-por-scout?unidadId=${unidadId}`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => setResumen(res.data))
      .catch(() => alert("Error al cargar resumen de especialidades"));
  }, []);

  return (
    <div className="min-h-screen bg-white text-gray-800 pb-20">
      {/* Menú superior fijo para pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-4xl mx-auto mt-6 px-4">
        <h2 className="text-2xl font-bold mb-6">Resumen de Especialidades por Scout</h2>

        {resumen.length === 0 ? (
          <p>No hay datos disponibles.</p>
        ) : (
          <div className="space-y-6">
            {resumen.map((scout) => (
              <div key={scout.scoutId} className="border rounded-xl p-4 shadow bg-white">
                <h3 className="text-lg font-semibold mb-2">{scout.nombreCompleto}</h3>
                {scout.especialidades.length === 0 ? (
                  <p className="text-sm text-gray-600">No inició ninguna especialidad.</p>
                ) : (
                  <ul className="list-disc list-inside space-y-1">
                    {scout.especialidades.map((esp) => (
                      <li key={esp.especialidadId}>
                        <strong>{esp.nombre}</strong>: {esp.aprobados} de {esp.seleccionados} aprobados
                        {esp.cumplida && <span className="text-green-600 ml-2">✔ Completada</span>}
                      </li>
                    ))}
                  </ul>
                )}
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Menú inferior fijo para móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}
