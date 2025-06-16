import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import MenuFijo from "../../components/MenuFijo";

export default function EspecialidadesResumenDirigente() {
  const [scouts, setScouts] = useState([]);
  const token = localStorage.getItem("token");
  const unidadId = localStorage.getItem("unidadId");
  const navigate = useNavigate();

  useEffect(() => {
    fetch(`/api/especialidades/resumen-por-scout?unidadId=${unidadId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
      .then((res) => res.json())
      .then(setScouts)
      .catch((err) => console.error("Error al cargar resumen", err));
  }, [unidadId, token]);

  return (
    <div className="min-h-screen bg-white text-gray-800 flex flex-col pb-20">
      {/* Menú fijo superior para pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-3xl mx-auto mt-6 px-4">
        <h2 className="text-2xl font-bold mb-6">Resumen de Especialidades por Scout</h2>

        {scouts.length === 0 ? (
          <p>No hay datos de especialidades para mostrar.</p>
        ) : (
          <ul className="space-y-4">
            {scouts.map((scout) => (
              <li key={scout.scoutId} className="p-4 border rounded-xl bg-white shadow">
                <h3 className="font-semibold text-lg mb-2">{scout.nombreCompleto}</h3>
                {scout.especialidades.length === 0 ? (
                  <p className="text-sm text-gray-600">Sin especialidades aún.</p>
                ) : (
                  <ul className="space-y-1">
                    {scout.especialidades.map((esp) => (
                      <li key={esp.especialidadId} className="flex justify-between text-sm">
                        <span>{esp.nombre}</span>
                        <span
                          className={
                            esp.cumplida
                              ? "text-green-600 font-semibold"
                              : "text-yellow-600 font-medium"
                          }
                        >
                          {esp.seleccionados} / {esp.aprobados} aprobados{" "}
                          {esp.cumplida ? "✔ Completada" : "(En progreso)"}
                        </span>
                      </li>
                    ))}
                  </ul>
                )}
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* Menú fijo inferior en móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}
