<template>
    <div class="container p-10">
        <div class="relative">
            <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
                <svg class="w-4 h-4 text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                    <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"/>
                </svg>
            </div>
            <input
                type="search"
                id="search"
                v-model="searchQuery"
                class="block w-full p-4 ps-10 bg-stone-950 text-gray-400 border border-gray-400 font-mono rounded-lg focus:ring-blue-900 focus:border-blue-900"
                placeholder="Da game finder huzzaa"
                required
            />
        </div>

        <ul class="mt-6">
            <li
                v-for="game in filteredGames"
                :key="game.game_id"
                class="p-4 bg-stone-800 text-gray-300 rounded-lg mb-2 cursor-pointer hover:bg-stone-700"
                @click="goToGamePage(game.game_id)"
            >
                {{ game.game_name }}
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
button {
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
    box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.2);
}
button:active {
    transform: translateY(2px);
    box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.2);
}
</style>