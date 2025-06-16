import React from "react";
import EventosDirigentePanel from "@/components/dirigente/EventosDirigentePanel";
import MenuFijo from "@/components/MenuFijo";

const PaginaEventosDirigente = () => {
  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo superior en pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-5xl mx-auto px-4 pt-6">
        <EventosDirigentePanel />
      </div>

      {/* Menú fijo inferior en móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
};

export default PaginaEventosDirigente;
