import { useNavigate } from "react-router-dom";

const BotonVolverPanel = () => {
  const navigate = useNavigate();
  const tipo = localStorage.getItem("tipo");

  const volverAlPanel = () => {
    if (tipo === "Scout") {
      navigate("/panel-scout");
    } else if (tipo === "Dirigente") {
      navigate("/panel-dirigente");
    }
  };

  return (
    <button
      onClick={volverAlPanel}
      className="absolute top-4 right-4 bg-gray-200 hover:bg-gray-300 text-sm text-black px-4 py-1 rounded shadow"
    >
      ← Volver al Panel
    </button>
  );
};

export default BotonVolverPanel;
