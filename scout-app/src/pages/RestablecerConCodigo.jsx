import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

export default function RestablecerConCodigo() {
  const [codigo, setCodigo] = useState("");
  const [nuevaPassword, setNuevaPassword] = useState("");
  const [mensaje, setMensaje] = useState(null);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const res = await axios.post("/api/passwordreset/restablecer", {
        codigo,
        nuevaPassword,
      });

      setMensaje({ tipo: "exito", texto: res.data.mensaje });

      setTimeout(() => navigate("/login"), 3000);
    } catch (err) {
      setMensaje({
        tipo: "error",
        texto:
          err.response?.data?.mensaje || "Error al restablecer la contraseña",
      });
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-purple-100 p-4">
      <div className="w-full max-w-md p-6 bg-transparent rounded shadow-none">
        <h2 className="text-2xl font-bold mb-6 text-center text-purple-800">
          Restablecer Contraseña
        </h2>

        <form onSubmit={handleSubmit}>
          <label className="block mb-2 font-semibold text-purple-800">
            Código de recuperación
          </label>
          <input
            type="text"
            className="w-full p-3 mb-4 rounded border-none bg-white text-gray-800"
            value={codigo}
            onChange={(e) => setCodigo(e.target.value)}
            required
          />

          <label className="block mb-2 font-semibold text-purple-800">
            Nueva contraseña
          </label>
          <input
            type="password"
            className="w-full p-3 mb-4 rounded border-none bg-white text-gray-800"
            value={nuevaPassword}
            onChange={(e) => setNuevaPassword(e.target.value)}
            required
          />

          <button
            type="submit"
            className="w-full py-3 rounded font-semibold bg-purple-200 text-purple-800 hover:bg-white hover:text-purple-700 transition"
          >
            Confirmar
          </button>
        </form>

        {mensaje && (
          <div
            className={`mt-4 p-3 text-sm rounded text-center ${
              mensaje.tipo === "exito"
                ? "bg-green-100 text-green-800"
                : "bg-red-100 text-red-800"
            }`}
          >
            {mensaje.texto}
          </div>
        )}
      </div>
    </div>
  );
}
