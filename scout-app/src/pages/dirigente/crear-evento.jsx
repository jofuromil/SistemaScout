import React from "react";
import CrearEvento from "@/components/dirigente/CrearEvento";
import MenuFijo from "@/components/MenuFijo";

const PaginaCrearEvento = () => {
  return (
    <div className="min-h-screen bg-white pb-20">
      {/* Menú fijo en la parte superior para pantallas grandes */}
      <div className="hidden lg:block fixed top-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>

      <div className="max-w-4xl mx-auto px-4 pt-6">
        <CrearEvento />
      </div>

      {/* Menú fijo en la parte inferior para móviles */}
      <div className="lg:hidden fixed bottom-0 left-0 right-0 z-50">
        <MenuFijo />
      </div>
    </div>
  );
};

export default PaginaCrearEvento;
