import React, { useEffect, useState } from "react";
import axios from "axios";
import { useParams } from "react-router-dom";
import MenuFijo from "@/components/MenuFijo";

export default function KardexScout() {
  const { scoutId } = useParams();
  const [usuario, setUsuario] = useState(null);
  const [resumenObjetivos, setResumenObjetivos] = useState([]);
  const [especialidades, setEspecialidades] = useState([]);
  const token = localStorage.getItem("token");
  const usuarioId = scoutId || localStorage.getItem("usuarioId");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const config = { headers: { Authorization: `Bearer ${token}` } };

        const userRes = scoutId
          ? await axios.get(`/api/users/${scoutId}`, config)
          : await axios.get("/api/users/me", config);
        setUsuario(userRes.data);

        const objRes = await axios.get(
          `/api/Objetivo/resumen-scout?usuarioId=${usuarioId}`,
          config
        );
        setResumenObjetivos(objRes.data);

        const espRes = await axios.get(
          scoutId
            ? `/api/Especialidades/mis-avances?scoutId=${scoutId}`
            : "/api/Especialidades/mis-avances",
          config
        );
        setEspecialidades(espRes.data);
      } catch (err) {
        console.error("Error cargando Kardex:", err);
      }
    };

    fetchData();
  }, [scoutId]);

  if (!usuario) return <p className="p-4">⏳ Cargando Kardex...</p>;

  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo superior */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-4xl mx-auto px-4 pt-6">
        <h1 className="text-2xl font-bold mb-4">📋 Kardex Scout</h1>

        {/* Datos personales */}
        <section className="mb-6">
          <h2 className="text-xl font-semibold mb-2">🧍 Datos Personales</h2>
          <p><strong>Nombre:</strong> {usuario.nombreCompleto}</p>
          <p><strong>Unidad:</strong> {usuario.unidad?.nombre || "-"}</p>
          <p><strong>Rama:</strong> {usuario.unidad?.rama || "-"}</p>
          <p><strong>Grupo Scout:</strong> {usuario.unidad?.grupoScout || usuario.unidad?.grupo?.nombre || "-"}</p>
          <p><strong>Distrito:</strong> {usuario.unidad?.distrito || "-"}</p>
        </section>

        {/* Resumen de objetivos */}
        <section className="mb-6">
          <h2 className="text-xl font-semibold mb-2">🎯 Resumen de Objetivos</h2>
          <div className="overflow-x-auto">
            <table className="min-w-full bg-white rounded shadow text-sm">
              <thead className="bg-blue-100 text-blue-800">
                <tr>
                  <th className="px-4 py-2 text-left">Nivel</th>
                  <th className="px-4 py-2 text-left">Área</th>
                  <th className="px-4 py-2 text-center">Total</th>
                  <th className="px-4 py-2 text-center">Validados</th>
                  <th className="px-4 py-2 text-center">Pendientes</th>
                </tr>
              </thead>
              <tbody>
                {resumenObjetivos.map((item, i) => (
                  <tr key={i} className="border-t">
                    <td className="px-4 py-2">{item.nivelProgresion || "—"}</td>
                    <td className="px-4 py-2">{item.areaCrecimiento}</td>
                    <td className="px-4 py-2 text-center">{item.total}</td>
                    <td className="px-4 py-2 text-center">{item.validados}</td>
                    <td className="px-4 py-2 text-center">{item.pendientes}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>

        {/* Resumen de especialidades */}
        <section className="mb-10">
          <h2 className="text-xl font-semibold mb-2">🏅 Resumen de Especialidades</h2>
          {especialidades.length === 0 ? (
            <p>No se han iniciado especialidades.</p>
          ) : (
            <table className="min-w-full bg-white rounded shadow text-sm">
              <thead className="bg-purple-100 text-purple-800">
                <tr>
                  <th className="px-4 py-2 text-left">Especialidad</th>
                  <th className="px-4 py-2 text-center">Seleccionados</th>
                  <th className="px-4 py-2 text-center">Aprobados</th>
                  <th className="px-4 py-2 text-center">Estado</th>
                </tr>
              </thead>
              <tbody>
                {especialidades.map((esp) => (
                  <tr key={esp.especialidadId} className="border-t">
                    <td className="px-4 py-2">{esp.nombre}</td>
                    <td className="px-4 py-2 text-center">{esp.seleccionados}</td>
                    <td className="px-4 py-2 text-center">{esp.aprobados}</td>
                    <td className="px-4 py-2 text-center">
                      {esp.cumplida ? (
                        <span className="text-green-600 font-semibold">✔ Completada</span>
                      ) : (
                        <span className="text-yellow-600">En proceso</span>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </section>
      </div>

      {/* Menú fijo inferior en móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
}
