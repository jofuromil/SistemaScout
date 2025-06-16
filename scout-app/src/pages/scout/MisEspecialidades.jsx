import { useEffect, useState } from "react";
import MenuFijo from "@/components/MenuFijo";

export default function MisEspecialidades() {
  const [especialidades, setEspecialidades] = useState([]);
  const token = localStorage.getItem("token");

  useEffect(() => {
    fetch("/api/Especialidades/mis-avances", {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
      .then((res) => {
        if (!res.ok) throw new Error("Error al obtener avances");
        return res.json();
      })
      .then((data) => setEspecialidades(data))
      .catch((err) => console.error(err));
  }, [token]);

  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo superior en pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-3xl mx-auto pt-6 px-4">
        <h2 className="text-2xl font-bold mb-4">⭐ Mis Especialidades</h2>

        {especialidades.length === 0 ? (
          <p>No has iniciado ninguna especialidad aún.</p>
        ) : (
          <ul className="grid gap-3">
            {especialidades.map((esp) => (
              <li
                key={esp.especialidadId}
                className="p-4 border rounded bg-gray-50 shadow flex justify-between items-center"
              >
                <div>
                  <div className="font-semibold">{esp.nombre}</div>
                  <div className="text-sm text-gray-600">
                    Requisitos: {esp.seleccionados} seleccionados / {esp.aprobados} aprobados
                  </div>
                </div>
                {esp.cumplida ? (
                  <span className="text-green-600 font-bold">✔ Cumplida</span>
                ) : (
                  <span className="text-yellow-600 font-semibold">En progreso</span>
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
