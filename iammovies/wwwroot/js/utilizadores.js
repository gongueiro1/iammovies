
document.addEventListener("DOMContentLoaded", () => {
    // Apagar utilizador
    document.querySelectorAll(".btn-apagar").forEach(button => {
        button.addEventListener("click", async (e) => {
            const userId = button.dataset.userid;
            const confirmDelete = confirm("Tens a certeza que queres apagar este utilizador?");
            if (!confirmDelete) return;

            try {
                const response = await fetch(`/api/utilizadoresapi/${userId}`, {
                    method: "DELETE"
                });

                if (response.ok) {
                    document.getElementById(`user-row-${userId}`).remove();
                    alert("Utilizador apagado com sucesso.");
                } else {
                    alert("Erro ao apagar utilizador.");
                }
            } catch (error) {
                console.error("Erro:", error);
                alert("Erro na comunicação com a API.");
            }
        });
    });

    // Editar utilizador
    document.querySelectorAll(".btn-editar").forEach(button => {
        button.addEventListener("click", async () => {
            const userId = button.dataset.userid;
            const nome = prompt("Novo nome:");
            const email = prompt("Novo email:");
            const role = prompt("Nova role (admin/user):");

            if (!nome || !email || !role) {
                alert("Preenchimento inválido.");
                return;
            }

            try {
                const response = await fetch(`/api/utilizadoresapi/${userId}`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ nome, email, role })
                });

                if (response.ok) {
                    alert("Utilizador atualizado com sucesso.");
                    location.reload();
                } else {
                    alert("Erro ao atualizar utilizador.");
                }
            } catch (error) {
                console.error("Erro:", error);
                alert("Erro na comunicação com a API.");
            }
        });
    });
});
