import { useNavigate } from "react-router-dom";
import { UserPlus, LogIn } from "lucide-react";

function Inicio() {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex flex-col justify-center items-center bg-purple-600 text-white px-6 py-10">
      <h1 className="text-4xl font-bold mb-10">Bienvenido</h1>

      <img
        src="/logo-scout-color.png" // Asegúrate de tener esta imagen en public o ajusta el path
        alt="Logo Scout"
        className="w-60 h-60 mb-12"
      />

      <div className="flex flex-col gap-6 w-full max-w-xs">
        <button
          onClick={() => navigate("/registro")}
          className="flex items-center justify-center gap-3 border-2 border-white text-white py-3 rounded-full text-lg font-semibold hover:bg-white hover:text-purple-600 transition"
        >
          <UserPlus className="w-6 h-6" />
          Registrarse
        </button>

        <button
          onClick={() => navigate("/login")}
          className="flex items-center justify-center gap-3 border-2 border-white text-white py-3 rounded-full text-lg font-semibold hover:bg-white hover:text-purple-600 transition"
        >
          <LogIn className="w-6 h-6" />
          Iniciar Sesión
        </button>
        <p className="w-full text-center">by Dei</p>
      </div>
    </div>
  );
}

export default Inicio;
