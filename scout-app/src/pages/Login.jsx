import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { LogIn } from "lucide-react";

function Login() {
  const [correo, setCorreo] = useState("");
  const [clave, setClave] = useState("");
  const [mensaje, setMensaje] = useState("");

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMensaje("");

    try {
      const res = await axios.post("http://localhost:8080/api/users/login", {
        correo,
        password: clave,
      });

      const data = res.data;

      localStorage.setItem("token", data.token);
      localStorage.setItem("usuarioId", data.user.id);
      localStorage.setItem("tipo", data.user.tipo);
      localStorage.setItem("rama", data.user.rama || "");
      localStorage.setItem("unidadId", data.user.unidadId || "");

      if (!data.user.unidadId) {
        navigate("/unidad");
      } else if (data.user.tipo.toLowerCase() === "dirigente") {
        navigate("/panel-dirigente");
      } else {
        navigate("/panel-scout");
      }
    } catch (err) {
      const msg =
        err.response?.data?.mensaje || "❌ Error de autenticación.";
      setMensaje(msg);
    }
  };

  return (
    <div className="min-h-screen bg-purple-600 text-white flex flex-col justify-center items-center px-6 py-10">
      <h1 className="text-3xl font-bold mb-8 flex items-center gap-2">
        <LogIn className="w-8 h-8" />
        Iniciar Sesión
      </h1>

      <form onSubmit={handleSubmit} className="w-full max-w-sm flex flex-col gap-4">
        <input
          type="text"
          placeholder="Correo electrónico"
          value={correo}
          onChange={(e) => setCorreo(e.target.value)}
          required
          className="p-3 rounded-lg bg-white text-black placeholder-gray-500"
        />
        <input
          type="password"
          placeholder="Contraseña"
          value={clave}
          onChange={(e) => setClave(e.target.value)}
          required
          className="p-3 rounded-lg bg-white text-black placeholder-gray-500"
        />

        <button
          type="submit"
          className="flex items-center justify-center gap-2 border-2 border-white text-white py-3 rounded-full text-lg font-semibold hover:bg-white hover:text-purple-700 transition"
        >
          Ingresar
        </button>

        <div className="text-center">
          <a
            href="/restablecer"
            className="text-sm text-white hover:underline"
          >
            ¿Olvidaste tu contraseña?
          </a>
        </div>

        {mensaje && (
          <div className="text-red-200 mt-2 text-center text-sm">{mensaje}</div>
        )}
      </form>
    </div>
  );
}

export default Login;
