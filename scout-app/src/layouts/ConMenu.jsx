import MenuFijo from "../components/MenuFijo";

export default function ConMenu({ children }) {
  return (
    <div className="min-h-screen bg-white text-gray-800 pb-24 px-4 pt-6">
      {children}
      <MenuFijo />
    </div>
  );
}
