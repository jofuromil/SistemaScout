import React from "react";

export default function ModalCodigoReset({ codigo, nombreUsuario, onClose }) {
  const copiarAlPortapapeles = () => {
    navigator.clipboard.writeText(codigo);
    alert("¡Código copiado al portapapeles!");
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 shadow-lg max-w-sm w-full">
        <h2 className="text-xl font-bold mb-4 text-center">Código de recuperación</h2>
        <p className="text-center text-sm mb-2 text-gray-700">
          Este código es para: <strong>{nombreUsuario}</strong>
        </p>
        <p className="text-center font-mono text-lg bg-gray-100 px-4 py-2 rounded border mb-4">
          {codigo}
        </p>
        <div className="flex justify-center gap-4">
          <button
            onClick={copiarAlPortapapeles}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            Copiar
          </button>
          <button
            onClick={onClose}
            className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500"
          >
            Cerrar
          </button>
        </div>
      </div>
    </div>
  );
}
