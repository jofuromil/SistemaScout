document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.getElementById("loginForm");

  loginForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    try {
      const response = await fetch("/api/users/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
      });

      if (!response.ok) {
        alert("Usuario o contraseña incorrectos.");
        return;
      }

      const data = await response.json();
      const token = data.token;

      // Decodificar el token para obtener el rol
      const payload = JSON.parse(atob(token.split('.')[1]));
      const rol = payload.role;

      // Guardamos el token para futuras peticiones
      localStorage.setItem("token", token);

      // Redirigimos según el rol
      if (rol === "Dirigente") {
        window.location.href = "panel-dirigente.html";
      } else if (rol === "Scout") {
        window.location.href = "panel-scout.html";
      } else {
        alert("Rol no reconocido.");
      }
    } catch (error) {
      console.error("Error durante el login:", error);
      alert("Error de red. Intenta más tarde.");
    }
  });
});
