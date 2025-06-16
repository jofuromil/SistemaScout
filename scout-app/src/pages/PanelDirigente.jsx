import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import MenuFijo from "../components/MenuFijo";

function PanelDirigente() {
  const [codigoUnidad, setCodigoUnidad] = useState("");
  const [nombreUnidad, setNombreUnidad] = useState("");
  const [ramaUnidad, setRamaUnidad] = useState("");
  const [nombreDirigente, setNombreDirigente] = useState("");
  const [unidadInfoVisible, setUnidadInfoVisible] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/login");
      return;
    }

    axios
      .get("http://localhost:8080/api/users/me", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => {
        const user = res.data;
        setNombreDirigente(user.nombreCompleto);
        if (user?.unidad?.codigoUnidad) {
          setCodigoUnidad(user.unidad.codigoUnidad);
          setNombreUnidad(user.unidad.nombre);
          setRamaUnidad(user.unidad.rama);
          setUnidadInfoVisible(true);
          localStorage.setItem("unidadId", user.unidad.id);
          localStorage.setItem("codigoUnidad", user.unidad.codigoUnidad);
        }
      })
      .catch(() => {
        alert("Error al cargar los datos del dirigente.");
        navigate("/login");
      });
  }, [navigate]);

  return (
    <div className="min-h-screen bg-white pb-20 flex flex-col">
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-2xl mx-auto p-6 pt-6">
        <h1 className="text-3xl font-bold mb-6">🎖️ Panel del Dirigente</h1>

        {unidadInfoVisible && (
          <div className="bg-purple-100 p-4 rounded-xl mb-6">
            <p><strong>Dirigente:</strong> {nombreDirigente}</p>
            <p><strong>Nombre de la unidad:</strong> {nombreUnidad}</p>
            <p><strong>Rama:</strong> {ramaUnidad}</p>
            <p><strong>Código de la unidad:</strong> {codigoUnidad}</p>
          </div>
        )}

        <div className="mb-6">
          <h2 className="text-xl font-semibold mb-2">Unidad Scout</h2>
          <button onClick={() => navigate("/miembros-unidad")} className="border-2 border-purple-600 text-purple-700 px-4 py-2 rounded-full w-full mb-2">
            Administrar Unidad
          </button>
          <button onClick={() => navigate("/dirigente/scouts-unidad")} className="border-2 border-purple-600 text-purple-700 px-4 py-2 rounded-full w-full">
            Scouts de la Unidad
          </button>
        </div>

        <div className="mb-6">
          <h2 className="text-xl font-semibold mb-2">Objetivos Educativos</h2>
          <button onClick={() => navigate("/dirigente/validar-objetivos")} className="border-2 border-purple-600 text-purple-700 px-4 py-2 rounded-full w-full">
            Validar Objetivos de Scouts
          </button>
        </div>

        <div className="mb-6">
          <h2 className="text-xl font-semibold mb-2">Especialidades</h2>
          <button
            onClick={() => navigate("/dirigente/especialidades")}
            className="border-2 border-purple-600 text-purple-700 px-4 py-2 rounded-full w-full mb-2"
          >
            Ver especialidades para aprobar
          </button>
          <button
            onClick={() => navigate("/dirigente/especialidades/resumen")}
            className="border-2 border-purple-600 text-purple-700 px-4 py-2 rounded-full w-full"
          >
            Ver especialidades de scouts
          </button>
        </div>

        <div className="mb-6">
          <h2 className="text-xl font-semibold mb-2">Eventos</h2>
          <button
            onClick={() => navigate("/dirigente/eventos")}
            className="border-2 border-purple-600 text-purple-700 px-4 py-2 rounded-full w-full"
          >
            📋 Gestionar eventos de mi unidad
          </button>
        </div>

        <div className="mb-6">
          <h2 className="text-xl font-semibold mb-2">Comunicación</h2>
          <button
            onClick={() => navigate("/mensajes-unidad")}
            className="border-2 border-purple-600 text-purple-700 px-4 py-2 rounded-full w-full mb-2"
          >
            Mensajes a la Unidad
          </button>
          <button
            onClick={() => navigate("/ver-mensajes-unidad")}
            className="border-2 border-purple-600 text-purple-700 px-4 py-2 rounded-full w-full"
          >
            Ver Mensajes de la Unidad
          </button>
        </div>

        <div>
          <h2 className="text-xl font-semibold mb-2">Cuenta</h2>
          <button
            onClick={() => navigate("/perfil")}
            className="border-2 border-gray-600 text-gray-700 px-4 py-2 rounded-full w-full mb-2"
          >
            Ver / Editar Perfil
          </button>
          <button
            onClick={() => navigate("/login")}
            className="border-2 border-red-600 text-red-700 px-4 py-2 rounded-full w-full"
          >
            Cerrar sesión
          </button>
        </div>
      </div>

      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}

export default PanelDirigente;
