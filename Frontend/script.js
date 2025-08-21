const API_BASE_URL = 'http://localhost:5000/api';

// Funções de UI
function showSection(sectionName) {
    document.querySelectorAll('.section').forEach(section => {
        section.classList.remove('active');
    });
    document.querySelectorAll('.nav-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    
    document.getElementById(`${sectionName}-section`).classList.add('active');
    document.querySelector(`button[onclick="showSection('${sectionName}')"]`).classList.add('active');
    
    // Carregar dados da seção
    switch(sectionName) {
        case 'livros':
            carregarLivros();
            break;
        case 'usuarios':
            carregarUsuarios();
            break;
        case 'emprestimos':
            carregarEmprestimos();
            break;
    }
}

function showModal(modalId) {
    document.getElementById(modalId).style.display = 'block';
    
    // Preencher selects do modal de empréstimo
    if (modalId === 'add-emprestimo') {
        preencherSelectsEmprestimo();
    }
}

function closeModal(modalId) {
    document.getElementById(modalId).style.display = 'none';
}

function showLoading() {
    document.getElementById('loading').classList.add('active');
}

function hideLoading() {
    document.getElementById('loading').classList.remove('active');
}

// API Functions
async function fetchData(endpoint) {
    try {
        showLoading();
        const response = await fetch(`${API_BASE_URL}${endpoint}`);
        if (!response.ok) throw new Error('Erro na requisição');
        return await response.json();
    } catch (error) {
        console.error('Erro:', error);
        alert('Erro ao carregar dados. Verifique se a API está rodando.');
        return [];
    } finally {
        hideLoading();
    }
}

async function postData(endpoint, data) {
    try {
        showLoading();
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });
        
        if (!response.ok) {
            const error = await response.text();
            throw new Error(error);
        }
        
        return await response.json();
    } catch (error) {
        console.error('Erro:', error);
        alert(`Erro: ${error.message}`);
        return null;
    } finally {
        hideLoading();
    }
}

// Livros
async function carregarLivros() {
    const searchTerm = document.getElementById('search-livros').value;
    const disponibilidade = document.getElementById('disponibilidade-filter').value;
    
    let endpoint = '/livros';
    if (searchTerm) {
        endpoint = `/livros/pesquisar?termo=${encodeURIComponent(searchTerm)}`;
    }
    
    const livros = await fetchData(endpoint);
    
    // Filtrar por disponibilidade
    let livrosFiltrados = livros;
    if (disponibilidade === 'disponiveis') {
        livrosFiltrados = livros.filter(l => l.disponivel);
    } else if (disponibilidade === 'indisponiveis') {
        livrosFiltrados = livros.filter(l => !l.disponivel);
    }
    
    exibirLivros(livrosFiltrados);
}

function exibirLivros(livros) {
    const grid = document.getElementById('livros-grid');
    grid.innerHTML = '';

    if (livros.length === 0) {
        grid.innerHTML = '<div class="no-data">Nenhum livro encontrado</div>';
        return;
    }

    livros.forEach(livro => {
        const card = document.createElement('div');
        card.className = 'livro-card';
        card.innerHTML = `
            <h3>${livro.titulo}</h3>
            <p><i class="fas fa-user"></i> ${livro.autor}</p>
            <p><i class="fas fa-book"></i> ${livro.genero}</p>
            <p><i class="fas fa-calendar"></i> ${livro.anoPublicacao}</p>
            <p><i class="fas fa-barcode"></i> ${livro.isbn}</p>
            <span class="status ${livro.disponivel ? 'disponivel' : 'indisponivel'}">
                ${livro.disponivel ? '📚 Disponível' : '⏳ Emprestado'}
            </span>
        `;
        grid.appendChild(card);
    });
}

async function adicionarLivro(event) {
    event.preventDefault();
    
    const livro = {
        titulo: document.getElementById('livro-titulo').value,
        autor: document.getElementById('livro-autor').value,
        isbn: document.getElementById('livro-isbn').value,
        anoPublicacao: parseInt(document.getElementById('livro-ano').value),
        genero: document.getElementById('livro-genero').value
    };

    const resultado = await postData('/livros', livro);
    if (resultado) {
        alert('Livro adicionado com sucesso!');
        closeModal('add-livro');
        document.getElementById('livro-form').reset();
        carregarLivros();
    }
}

// Usuários
async function carregarUsuarios() {
    const searchTerm = document.getElementById('search-usuarios').value;
    
    let endpoint = '/usuarios';
    if (searchTerm) {
        endpoint = `/usuarios/pesquisar?termo=${encodeURIComponent(searchTerm)}`;
    }
    
    const usuarios = await fetchData(endpoint);
    exibirUsuarios(usuarios);
}

function exibirUsuarios(usuarios) {
    const list = document.getElementById('usuarios-list');
    list.innerHTML = '';

    if (usuarios.length === 0) {
        list.innerHTML = '<div class="no-data">Nenhum usuário encontrado</div>';
        return;
    }

    usuarios.forEach(usuario => {
        const card = document.createElement('div');
        card.className = 'usuario-card';
        card.innerHTML = `
            <div class="usuario-info">
                <h4>${usuario.nome}</h4>
                <p><i class="fas fa-envelope"></i> ${usuario.email}</p>
                <p><i class="fas fa-phone"></i> ${usuario.telefone}</p>
                <p><i class="fas fa-calendar"></i> Cadastro: ${new Date(usuario.dataCadastro).toLocaleDateString()}</p>
            </div>
        `;
        list.appendChild(card);
    });
}

async function adicionarUsuario(event) {
    event.preventDefault();
    
    const usuario = {
        nome: document.getElementById('usuario-nome').value,
        email: document.getElementById('usuario-email').value,
        telefone: document.getElementById('usuario-telefone').value
    };

    const resultado = await postData('/usuarios', usuario);
    if (resultado) {
        alert('Usuário adicionado com sucesso!');
        closeModal('add-usuario');
        document.getElementById('usuario-form').reset();
        carregarUsuarios();
    }
}

// Empréstimos
async function carregarEmprestimos() {
    const status = document.getElementById('status-filter').value;
    
    let endpoint = '/emprestimos';
    if (status === 'ativos') {
        endpoint = '/emprestimos/ativos';
    }
    
    const emprestimos = await fetchData(endpoint);
    
    // Filtrar por status se necessário
    let emprestimosFiltrados = emprestimos;
    if (status === 'devolvidos') {
        emprestimosFiltrados = emprestimos.filter(e => e.dataDevolucaoReal);
    }
    
    exibirEmprestimos(emprestimosFiltrados);
}

function exibirEmprestimos(emprestimos) {
    const list = document.getElementById('emprestimos-list');
    list.innerHTML = '';

    if (emprestimos.length === 0) {
        list.innerHTML = '<div class="no-data">Nenhum empréstimo encontrado</div>';
        return;
    }

    emprestimos.forEach(emprestimo => {
        const card = document.createElement('div');
        card.className = 'emprestimo-card';
        card.innerHTML = `
            <div class="emprestimo-info">
                <h4>📖 ${emprestimo.livroTitulo || 'Livro desconhecido'}</h4>
                <p><i class="fas fa-user"></i> ${emprestimo.usuarioNome || 'Usuário desconhecido'}</p>
                <p><i class="fas fa-calendar-check"></i> Empréstimo: ${new Date(emprestimo.dataEmprestimo).toLocaleDateString()}</p>
                <p><i class="fas fa-calendar-times"></i> Devolução prevista: ${new Date(emprestimo.dataDevolucaoPrevista).toLocaleDateString()}</p>
                ${emprestimo.dataDevolucaoReal ? 
                    `<p><i class="fas fa-check-circle"></i> Devolvido em: ${new Date(emprestimo.dataDevolucaoReal).toLocaleDateString()}</p>` : 
                    `<p><i class="fas fa-clock"></i> Status: Em andamento</p>`
                }
            </div>
        `;
        list.appendChild(card);
    });
}

async function preencherSelectsEmprestimo() {
    const selectLivro = document.getElementById('emprestimo-livro');
    const selectUsuario = document.getElementById('emprestimo-usuario');
    
    // Carregar livros disponíveis
    const livros = await fetchData('/livros/disponiveis');
    selectLivro.innerHTML = '<option value="">Selecione um livro</option>';
    livros.forEach(livro => {
        const option = document.createElement('option');
        option.value = livro.id;
        option.textContent = `${livro.titulo} - ${livro.autor}`;
        selectLivro.appendChild(option);
    });
    
    // Carregar usuários
    const usuarios = await fetchData('/usuarios');
    selectUsuario.innerHTML = '<option value="">Selecione um usuário</option>';
    usuarios.forEach(usuario => {
        const option = document.createElement('option');
        option.value = usuario.id;
        option.textContent = `${usuario.nome} - ${usuario.email}`;
        selectUsuario.appendChild(option);
    });
}

async function realizarEmprestimo(event) {
    event.preventDefault();
    
    const emprestimo = {
        livroId: parseInt(document.getElementById('emprestimo-livro').value),
        usuarioId: parseInt(document.getElementById('emprestimo-usuario').value),
        diasEmprestimo: parseInt(document.getElementById('emprestimo-dias').value)
    };

    const resultado = await postData('/emprestimos', emprestimo);
    if (resultado) {
        alert('Empréstimo realizado com sucesso!');
        closeModal('add-emprestimo');
        document.getElementById('emprestimo-form').reset();
        carregarEmprestimos();
        carregarLivros(); // Atualizar status dos livros
    }
}

// Event Listeners
document.addEventListener('DOMContentLoaded', function() {
    // Carregar dados iniciais
    carregarLivros();
    
    // Fechar modal clicando fora
    window.onclick = function(event) {
        document.querySelectorAll('.modal').forEach(modal => {
            if (event.target === modal) {
                modal.style.display = 'none';
            }
        });
    };
});