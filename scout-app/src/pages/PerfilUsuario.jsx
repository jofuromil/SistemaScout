import React, { useEffect, useState } from "react";
import axios from "axios";
import MenuFijo from "@/components/MenuFijo";

export default function PerfilUsuario() {
  const [perfil, setPerfil] = useState(null);
  const [editando, setEditando] = useState(false);
  const token = localStorage.getItem("token");

  useEffect(() => {
    axios
      .get("/api/users/perfil", {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => setPerfil(res.data))
      .catch(() => alert("Error al cargar perfil"));
  }, []);

  const handleChange = (e) => {
    setPerfil({ ...perfil, [e.target.name]: e.target.value });
  };

  const guardarCambios = () => {
    axios
      .put("/api/users/perfil", perfil, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then(() => {
        alert("Perfil actualizado correctamente");
        setEditando(false);
      })
      .catch(() => alert("Error al actualizar perfil"));
  };

  if (!perfil) return <p className="p-4">⏳ Cargando perfil...</p>;

  return (
    <div className="min-h-screen bg-white pb-20 relative">
      {/* Menú fijo superior */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-xl mx-auto p-6 pt-6">
        <h2 className="text-2xl font-bold mb-4">Mi Perfil</h2>

        <div className="grid grid-cols-1 gap-4">
          <Campo nombre="Nombre completo" valor={perfil.nombreCompleto} name="nombreCompleto" editable={editando} onChange={handleChange} />
          <Campo nombre="Fecha de nacimiento" valor={perfil.fechaNacimiento ? perfil.fechaNacimiento.slice(0, 10) : ""} name="fechaNacimiento" tipo="date" editable={editando} onChange={handleChange} />
          <Campo nombre="Teléfono" valor={perfil.telefono} name="telefono" editable={editando} onChange={handleChange} />
          <Campo nombre="Ciudad" valor={perfil.ciudad} name="ciudad" editable={editando} onChange={handleChange} />
          <Campo nombre="Dirección" valor={perfil.direccion} name="direccion" editable={editando} onChange={handleChange} />
          <Campo nombre="Género" valor={perfil.genero} name="genero" editable={editando} onChange={handleChange} />

          {perfil.tipo === "Scout" ? (
            <>
              <Campo nombre="Institución educativa" valor={perfil.institucionEducativa} name="institucionEducativa" editable={editando} onChange={handleChange} />
              <Campo nombre="Nivel de estudios" valor={perfil.nivelEstudios} name="nivelEstudios" editable={editando} onChange={handleChange} />
            </>
          ) : (
            <>
              <Campo nombre="Profesión" valor={perfil.profesion} name="profesion" editable={editando} onChange={handleChange} />
              <Campo nombre="Ocupación" valor={perfil.ocupacion} name="ocupacion" editable={editando} onChange={handleChange} />
            </>
          )}

          <div>
            <label className="font-semibold">Correo electrónico</label>
            <input
              className="w-full p-2 border rounded bg-gray-100"
              value={perfil.correo}
              disabled
            />
          </div>
        </div>

        <div className="mt-6 flex gap-4">
          {!editando ? (
            <button className="bg-blue-600 text-white px-4 py-2 rounded" onClick={() => setEditando(true)}>
              Editar perfil
            </button>
          ) : (
            <>
              <button className="bg-green-600 text-white px-4 py-2 rounded" onClick={guardarCambios}>
                Guardar cambios
              </button>
              <button className="bg-gray-400 text-white px-4 py-2 rounded" onClick={() => setEditando(false)}>
                Cancelar
              </button>
            </>
          )}
        </div>
      </div>

      {/* Menú fijo inferior en móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}

function Campo({ nombre, valor, name, editable, onChange, tipo = "text" }) {
  return (
    <div>
      <label className="font-semibold">{nombre}</label>
      <input
        type={tipo}
        className="w-full p-2 border rounded"
        name={name}
        value={valor !== null && valor !== undefined ? valor : ""}
        disabled={!editable}
        onChange={onChange}
      />
    </div>
  );
}
