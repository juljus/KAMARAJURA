<template>
  <div class="container p-10">
    <h1 class="text-4xl font-bold text-gray-300 mb-6">Create a New Game</h1>
    <form @submit.prevent="submitGame" class="space-y-6">
      <div>
        <label for="gameName" class="block text-gray-400 mb-2">Game Name</label>
        <input
          type="text"
          id="gameName"
          v-model="newGame.game_name"
          required
          class="w-full px-4 py-2 bg-stone-950 text-gray-400 border border-gray-400 rounded-lg focus:outline-none focus:ring focus:ring-blue-300"
        />
      </div>
      <div>
        <label for="gameDescription" class="block text-gray-400 mb-2">Game Description</label>
        <textarea
          id="gameDescription"
          v-model="newGame.game_description"
          required
          class="w-full px-4 py-2 bg-stone-950 text-gray-400 border border-gray-400 rounded-lg focus:outline-none focus:ring focus:ring-blue-300"
        ></textarea>
      </div>
      <button
        type="submit"
        class="px-6 py-2 text-gray-300 bg-green-700 rounded-md hover:bg-green-800 focus:outline-none focus:ring focus:ring-green-300"
      >
        Submit
      </button>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import axios from 'axios';

const newGame = ref({
  game_name: '',
  game_description: ''
});

const submitGame = async () => {
  try {
    const token = localStorage.getItem('authToken');
    if (!token) {
      alert('You must be logged in to add a game.');
      return;
    }

    await axios.post('http://localhost:5005/api/game', newGame.value, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });

    alert('Game added successfully!');
    newGame.value = { game_name: '', game_description: '' };
  } catch (error) {
    alert('Failed to add game. Please try again.');
    console.error(error);
  }
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