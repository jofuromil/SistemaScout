import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { UserPlus } from "lucide-react";

function Registro() {
  const [form, setForm] = useState({
    nombreCompleto: "",
    fechaNacimiento: "",
    correo: "",
    password: "",
    tipo: "",
  });

  const [mensaje, setMensaje] = useState("");
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMensaje("");

    if (form.password.length < 6) {
      setMensaje("❌ La contraseña debe tener al menos 6 caracteres.");
      return;
    }

    try {
      const response = await axios.post("http://localhost:8080/api/users/registrar", form);
      localStorage.setItem("tipoRegistro", form.tipo);
      alert("✅ Registro exitoso. Ya puedes iniciar sesión.");
      navigate("/login");
    } catch (error) {
      let msg = "❌ Error en el registro.";
      if (error.response?.data?.mensaje) {
        msg = `❌ ${error.response.data.mensaje}`;
      }
      setMensaje(msg);
    }
  };

  return (
    <div className="min-h-screen bg-purple-600 text-white flex flex-col justify-center items-center px-6 py-10">
      <h2 className="text-3xl font-bold mb-8 flex items-center gap-2">
        <UserPlus className="w-8 h-8" />
        Crear cuenta
      </h2>

      <form onSubmit={handleSubmit} className="w-full max-w-sm flex flex-col gap-4">
        <input
          name="nombreCompleto"
          placeholder="Nombre completo"
          value={form.nombreCompleto}
          onChange={handleChange}
          required
          className="p-3 rounded-lg bg-white text-black placeholder-gray-500"
        />

        <input
          name="fechaNacimiento"
          type="date"
          value={form.fechaNacimiento}
          onChange={handleChange}
          required
          className="p-3 rounded-lg bg-white text-black"
        />

        <input
          name="correo"
          type="email"
          placeholder="Correo electrónico"
          value={form.correo}
          onChange={handleChange}
          required
          className="p-3 rounded-lg bg-white text-black placeholder-gray-500"
        />

        <input
          name="password"
          type="password"
          placeholder="Contraseña"
          value={form.password}
          onChange={handleChange}
          required
          className="p-3 rounded-lg bg-white text-black placeholder-gray-500"
        />

        <select
          name="tipo"
          value={form.tipo}
          onChange={handleChange}
          required
          className="p-3 rounded-lg bg-white text-black"
        >
          <option value="">Selecciona un tipo</option>
          <option value="Scout">Scout</option>
          <option value="Dirigente">Dirigente</option>
        </select>

        <button
          type="submit"
          className="flex items-center justify-center gap-2 border-2 border-white text-white py-3 rounded-full text-lg font-semibold hover:bg-white hover:text-purple-700 transition"
        >
          Registrarse
        </button>

        {mensaje && <p className="text-sm text-center text-red-200">{mensaje}</p>}
      </form>
    </div>
  );
}

export default Registro;
