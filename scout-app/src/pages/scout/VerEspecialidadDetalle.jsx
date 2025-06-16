import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import MenuFijo from "@/components/MenuFijo";

export default function VerEspecialidadDetalle() {
  const { id } = useParams();
  const token = localStorage.getItem("token");

  const [especialidad, setEspecialidad] = useState(null);
  const [requisitos, setRequisitos] = useState([]);

  useEffect(() => {
    fetch(`http://localhost:8080/api/Especialidades/avance-especialidad?idEspecialidad=${id}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
      .then((res) => res.json())
      .then((data) => {
        setEspecialidad({
          nombre: data.nombre,
          descripcion: data.descripcion
        });
        setRequisitos(data.requisitos);
      })
      .catch((err) => console.error("Error al cargar especialidad", err));
  }, [id, token]);

  const marcarCumplido = (requisitoId) => {
    fetch("http://localhost:8080/api/Especialidades/requisito-cumplido", {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ requisitoId })
    })
      .then((res) => {
        if (!res.ok) throw new Error("Error al marcar requisito");
        setRequisitos((prev) =>
          prev.map((r) =>
            r.requisitoId === requisitoId ? { ...r, seleccionado: true } : r
          )
        );
      })
      .catch((err) => console.error(err));
  };

  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo superior para pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-3xl mx-auto pt-6 px-4">
        <h2 className="text-2xl font-bold mb-2">{especialidad?.nombre}</h2>
        <p className="mb-4">{especialidad?.descripcion}</p>

        {requisitos.length === 0 ? (
          <p className="text-gray-600">No hay requisitos disponibles.</p>
        ) : (
          <ul className="space-y-3">
            {requisitos.map((r) => (
              <li
                key={r.requisitoId}
                className="p-4 border rounded bg-gray-50 shadow flex justify-between items-center"
              >
                <span>{r.texto}</span>
                {r.seleccionado ? (
                  <span className="text-green-600 font-semibold">✔ Cumplido</span>
                ) : (
                  <button
                    onClick={() => marcarCumplido(r.requisitoId)}
                    className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
                  >
                    Marcar cumplido
                  </button>
                )}
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* Menú fijo inferior para móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}
