import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import Inicio from "./pages/Inicio"; // 👈 este es el correcto
import Registro from "./pages/Registro";
import PanelScout from "./pages/PanelScout";
import PanelDirigente from "./pages/PanelDirigente";
import Unidad from "./pages/Unidad";
import Objetivos from "./pages/Objetivos"; // asegúrate que la ruta sea correcta
import ObjetivosNivel from "./pages/ObjetivosNivel";
import ObjetivosScoutPanel from "./pages/ObjetivosScoutPanel";
import MiembrosUnidad from "./pages/MiembrosUnidad";
import EnviarMensajeUnidad from "./pages/EnviarMensajeUnidad";
import VerMensajesUnidad from "./components/dirigente/VerMensajesUnidad";
import MensajesRecibidosScout from "./pages/MensajesRecibidosScout";
import PaginaEventosDirigente from "./pages/dirigente/eventos";
import PaginaCrearEvento from "./pages/dirigente/crear-evento";
import EventosScout from "./pages/scout/EventosScout";
import RegistrarAvanceEspecialidad from "./pages/scout/RegistrarAvanceEspecialidad";
import VerEspecialidadDetalle from "./pages/scout/VerEspecialidadDetalle";
import MisEspecialidades from "./pages/scout/MisEspecialidades";
import EspecialidadesDirigentePanel from "./pages/dirigente/EspecialidadesDirigentePanel";
import ValidarEspecialidadScout from "./pages/dirigente/ValidarEspecialidadScout";
import EventoDetalleDirigente from "./pages/dirigente/EventoDetalleDirigente";
import PerfilUsuario from "./pages/PerfilUsuario";
import RestablecerConCodigo from "./pages/RestablecerConCodigo";
import ResumenEspecialidadesDirigente from "./pages/dirigente/ResumenEspecialidadesDirigente";
import MiembrosScoutsUnidad from "./pages/MiembrosScoutsUnidad";
import NotFound from "./pages/NotFound";
import ValidarObjetivosDirigente from "./pages/ValidarObjetivosDirigente"; // Asegúrate de tener esta línea en la parte superior
import KardexScout from "./pages/KardexScout";
import KardexScoutDirigente from "./pages/KardexScoutDirigente";



function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Inicio />} />
        <Route path="/login" element={<Login />} />
        <Route path="/registro" element={<Registro />} />
        <Route path="/unidad" element={<Unidad />} />
        <Route path="/panel-scout" element={<PanelScout />} />
        <Route path="/panel-dirigente" element={<PanelDirigente />} />
        <Route path="/objetivos" element={<Objetivos />} />
        <Route path="/objetivos-nivel" element={<ObjetivosNivel />} />
        <Route path="/scout/objetivos" element={<ObjetivosScoutPanel />} />
        <Route path="/miembros-unidad" element={<MiembrosUnidad />} />
        <Route path="*" element={<NotFound />} />
        <Route path="/mensajes-unidad" element={<EnviarMensajeUnidad />} />
        <Route path="/ver-mensajes-unidad" element={<VerMensajesUnidad />} />
        <Route path="/mensajes-recibidos" element={<MensajesRecibidosScout />} />
        <Route path="/dirigente/crear-evento" element={<PaginaCrearEvento />} />
        <Route path="/dirigente/eventos" element={<PaginaEventosDirigente />} />
        <Route path="/scout/eventos" element={<EventosScout />} />
        <Route path="/registrar-avance-especialidades" element={<RegistrarAvanceEspecialidad />} />
        <Route path="/especialidades/:id" element={<VerEspecialidadDetalle />} />
        <Route path="/mis-especialidades" element={<MisEspecialidades />} />
        <Route path="/dirigente/especialidades" element={<EspecialidadesDirigentePanel />} />
        <Route path="/dirigente/especialidades/:scoutId" element={<ValidarEspecialidadScout />} />
        <Route path="/dirigente/evento-detalle/:eventoId" element={<EventoDetalleDirigente />} />
        <Route path="/restablecer" element={<RestablecerConCodigo />} />
        <Route path="/dirigente/especialidades/resumen" element={<ResumenEspecialidadesDirigente />} />
        <Route path="/dirigente/objetivos-scout/:scoutId" element={<ObjetivosScoutPanel />} />
        <Route path="/dirigente/scouts-unidad" element={<MiembrosScoutsUnidad />} />
        <Route path="/perfil" element={<PerfilUsuario />} />
        <Route path="/dirigente/validar-objetivos" element={<ValidarObjetivosDirigente />} />
        <Route path="/kardex" element={<KardexScout />} /> 
        <Route path="/kardex-scout/:scoutId" element={<KardexScout />} />  
        <Route path="/dirigente/kardex/:scoutId" element={<KardexScoutDirigente />} />
      </Routes>
    </Router>
  );
}

export default App;
