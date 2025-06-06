<template>
    <div class="container p-10">
        <h1 class="text-4xl text-gray-300 mb-6 font-bold text-center">Find Your Favorite Games</h1>
        <div class="relative mb-8">
            <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
                <svg class="w-5 h-5 text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                    <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"/>
                </svg>
            </div>
            <input
                type="search"
                id="search"
                v-model="searchQuery"
                class="block w-full p-4 ps-10 bg-stone-950 text-gray-400 border border-gray-400 font-mono rounded-lg focus:ring-blue-900 focus:border-blue-900 shadow-md"
                placeholder="Search for games..."
                required
            />
        </div>

        <ul class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <li
                v-for="game in filteredGames"
                :key="game.game_id"
                class="p-6 bg-stone-800 text-gray-300 rounded-lg shadow-lg cursor-pointer hover:bg-stone-700 hover:shadow-xl transition-transform transform hover:-translate-y-1"
                @click="goToGamePage(game.game_id)"
            >
                <h2 class="text-xl font-semibold mb-2">{{ game.game_name }}</h2>
                <p class="text-gray-400 text-sm">Click to view details</p>
            </li>
        </ul>
    </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';

const searchQuery = ref('');
const games = ref([]);

// Fetch games from the backend
const fetchGames = async () => {
    console.log('Fetching games from backend...');
    try {
        const response = await axios.get('http://localhost:5005/api/game');
        console.log('Response received:', response);
        games.value = response.data;
        console.log('Games array after fetching:', games.value);
    } catch (error) {
        console.error('Error fetching games:', error);
    }
};

// Filter games based on the search query
const filteredGames = computed(() => {
    const filtered = games.value.filter(game =>
        game.game_name.toLowerCase().includes(searchQuery.value.toLowerCase())
    );
    console.log('Filtered games during rendering:', filtered);
    return filtered;
});

// Fetch games on component mount
fetchGames();

// Debugging: Log the games array when the component is mounted
onMounted(() => {
    console.log('Games array on mount:', games.value);
});

const goToGamePage = (gameId) => {
    console.log('Navigating to game page with ID:', gameId);
    window.location.href = `/game/${gameId}`;
};
</script>

<style scoped>
.container {
    max-width: 1200px;
    margin: 0 auto;
    background-color: #1a1a1a;
    border-radius: 8px;
    padding: 20px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

h1 {
    color: #e2e8f0;
    text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5);
}

input {
    transition: box-shadow 0.2s ease-in-out;
}
input:focus {
    box-shadow: 0 0 10px rgba(0, 0, 255, 0.5);
}

ul {
    display: grid;
    gap: 1.5rem;
}

li {
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
}
li:hover {
    transform: translateY(-5px);
    box-shadow: 0 6px 10px rgba(0, 0, 0, 0.2);
}

button {
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
    box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.2);
}
button:active {
    transform: translateY(2px);
    box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.2);
}

.search-container {
    display: flex;
    align-items: center;
    gap: 10px;
}

.search-bar {
    flex: 1;
    padding: 8px;
}

.add-game-button {
    padding: 8px 12px;
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

.add-game-form {
    margin-top: 20px;
    padding: 20px;
    background-color: #f8f9fa;
    border: 1px solid #ddd;
    border-radius: 4px;
}

.add-game-form h3 {
    margin-bottom: 15px;
}

.add-game-form label {
    display: block;
    margin-bottom: 5px;
}

.add-game-form input,
.add-game-form textarea {
    width: 100%;
    padding: 8px;
    margin-bottom: 10px;
    border: 1px solid #ddd;
    border-radius: 4px;
}

.add-game-form button {
    padding: 8px 12px;
    background-color: #28a745;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}
</style>