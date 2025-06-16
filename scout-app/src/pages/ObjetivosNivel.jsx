import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import MenuFijo from "@/components/MenuFijo";

function ObjetivosNivel() {
  const navigate = useNavigate();
  const rama = localStorage.getItem("rama");
  const [nivel, setNivel] = useState("");
  const [opciones, setOpciones] = useState([]);

  useEffect(() => {
    if (rama === "Lobatos") {
      setOpciones(["", "PATA TIERNA - SALTADOR", "RASTREADOR - CAZADOR"]);
    } else if (rama === "Exploradores") {
      setOpciones(["", "PISTA - SENDA", "RUMBO - TRAVESIA"]);
    } else {
      // Si no aplica nivel, redirigir directamente
      navigate("/objetivos");
    }
  }, [rama, navigate]);

  const continuar = () => {
    localStorage.setItem("nivelProgresion", nivel);
    navigate("/objetivos");
  };

  return (
    <div className="min-h-screen bg-white pb-20 flex flex-col items-center justify-center p-6 text-center">
      {/* Menú fijo superior en pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <h1 className="text-2xl font-bold mb-6">📘 Elegir Nivel de Progresión</h1>

      <select
        value={nivel}
        onChange={(e) => setNivel(e.target.value)}
        className="p-3 text-lg w-72 mb-4 border rounded"
      >
        {opciones.map((op, i) => (
          <option key={i} value={op}>
            {op === "" ? "Todos los niveles" : op}
          </option>
        ))}
      </select>

      <button
        onClick={continuar}
        className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700"
      >
        Continuar
      </button>

      {/* Menú fijo inferior en móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}

export default ObjetivosNivel;
