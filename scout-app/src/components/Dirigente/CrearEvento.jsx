import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const CrearEvento = () => {
  const navigate = useNavigate();
  const [nombre, setNombre] = useState("");
  const [lugar, setLugar] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [observaciones, setObservaciones] = useState("");
  const [fechaInicio, setFechaInicio] = useState("");
  const [fechaFin, setFechaFin] = useState("");
  const [codigoUnidad, setCodigoUnidad] = useState("");
  const [tipo, setTipo] = useState("Campamento");
  const [mensaje, setMensaje] = useState(null);

  const tipos = [
    "Campamento",
    "Excursion",
    "Caminata",
    "Servicio",
    "Curso",
    "Salida",
    "Proyecto",
  ];

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    try {
      await axios.post(
        "/api/Evento/unidad",
        {
          nombre: `${tipo} - ${nombre}`,
          lugar,
          descripcion,
          observaciones,
          fechaInicio,
          fechaFin,
          codigoUnidad,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      setMensaje("Evento creado exitosamente.");
      setTimeout(() => navigate("/dirigente/eventos"), 2000);
    } catch (error) {
      console.error("Error al crear evento:", error.response?.data || error.message);
      setMensaje(error.response?.data?.mensaje || "Error al crear el evento.");
    }
  };

  return (
    <div className="max-w-xl mx-auto mt-10 bg-white p-6 rounded shadow">
      <h2 className="text-2xl font-bold mb-4">Crear Evento de Unidad</h2>
      {mensaje && <div className="mb-4 text-red-500">{mensaje}</div>}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block font-semibold">Tipo de evento:</label>
          <select
            value={tipo}
            onChange={(e) => setTipo(e.target.value)}
            className="w-full border rounded p-2"
          >
            {tipos.map((t) => (
              <option key={t} value={t}>
                {t}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label className="block font-semibold">Nombre:</label>
          <input
            type="text"
            value={nombre}
            onChange={(e) => setNombre(e.target.value)}
            className="w-full border rounded p-2"
            placeholder="Ej. Exploradores Norte"
            required
          />
        </div>

        <div>
          <label className="block font-semibold">Lugar:</label>
          <input
            type="text"
            value={lugar}
            onChange={(e) => setLugar(e.target.value)}
            className="w-full border rounded p-2"
            placeholder="Ej. Parque Municipal"
            required
          />
        </div>

        <div>
          <label className="block font-semibold">Descripción:</label>
          <textarea
            value={descripcion}
            onChange={(e) => setDescripcion(e.target.value)}
            className="w-full border rounded p-2"
            required
          ></textarea>
        </div>

        <div>
          <label className="block font-semibold">Observaciones (opcional):</label>
          <textarea
            value={observaciones}
            onChange={(e) => setObservaciones(e.target.value)}
            className="w-full border rounded p-2"
          ></textarea>
        </div>

        <div>
          <label className="block font-semibold">Fecha de inicio:</label>
          <input
            type="date"
            value={fechaInicio}
            onChange={(e) => setFechaInicio(e.target.value)}
            className="w-full border rounded p-2"
            required
          />
        </div>

        <div>
          <label className="block font-semibold">Fecha de fin:</label>
          <input
            type="date"
            value={fechaFin}
            onChange={(e) => setFechaFin(e.target.value)}
            className="w-full border rounded p-2"
            required
          />
        </div>

        <div>
          <label className="block font-semibold">Código de unidad:</label>
          <input
            type="text"
            value={codigoUnidad}
            onChange={(e) => setCodigoUnidad(e.target.value)}
            className="w-full border rounded p-2"
            required
          />
        </div>

        <button
          type="submit"
          className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
        >
          Crear evento
        </button>
      </form>
    </div>
  );
};

export default CrearEvento;
