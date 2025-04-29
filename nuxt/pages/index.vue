<template>
    <div>
        <h1 class="p-10 text-6xl">teretere</h1>

        <!-- User Form -->
        <form @submit.prevent="submitUserForm">
            <h2>Create User</h2>
            <div>
                <label for="username">Username:</label>
                <input id="username" v-model="username" type="text" required />
            </div>
            <div>
                <label for="password">Password:</label>
                <input id="password" v-model="password" type="password" required />
            </div>
            <button type="submit">Submit</button>
        </form>

        <!-- Game Form -->
        <form @submit.prevent="submitGameForm">
            <h2>Create Game</h2>
            <div>
                <label for="gameName">Game Name:</label>
                <input id="gameName" v-model="gameName" type="text" required />
            </div>
            <div>
                <label for="gameDescription">Game Description:</label>
                <input id="gameDescription" v-model="gameDescription" type="text" required />
            </div>
            <button type="submit">Submit</button>
        </form>

        <!-- Rating Form -->
        <form @submit.prevent="submitRatingForm">
            <h2>Create Rating</h2>
            <div>
                <label for="gameId">Game ID:</label>
                <input id="gameId" v-model="gameId" type="number" required />
            </div>
            <div>
                <label for="userId">User ID:</label>
                <input id="userId" v-model="userId" type="number" required />
            </div>
            <div>
                <label for="ratingScore">Rating Score:</label>
                <input id="ratingScore" v-model="ratingScore" type="number" required />
            </div>
            <button type="submit">Submit</button>
        </form>

        <p v-if="message">{{ message }}</p>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import axios from 'axios';

// User Form
const username = ref('');
const password = ref('');

// Game Form
const gameName = ref('');
const gameDescription = ref('');

// Rating Form
const gameId = ref(null);
const userId = ref(null);
const ratingScore = ref(null);

const message = ref('');

const submitUserForm = async () => {
    try {
        const response = await axios.post('http://localhost:5005/api/User', {
            name: username.value,
            password: password.value
        });
        message.value = 'User created successfully!';
    } catch (error) {
        message.value = `Error: ${error.response?.data || error.message}`;
    }
};

const submitGameForm = async () => {
    try {
        const response = await axios.post('http://localhost:5005/api/Game', {
            gameName: gameName.value,
            gameDescription: gameDescription.value,
            gameImage: null // Placeholder for image data
        });
        message.value = 'Game created successfully!';
    } catch (error) {
        console.error('Error details:', error.response?.data?.errors || error.response || error);
        message.value = `Error: ${error.response?.data?.title || error.message}`;
    }
};

const submitRatingForm = async () => {
    try {
        const response = await axios.post('http://localhost:5005/api/Rating', {
            gameId: gameId.value,
            userId: userId.value,
            ratingTimestamp: new Date().toISOString(),
            ratingScore: ratingScore.value,
            ratingDescription: '' // Optional description
        });
        message.value = 'Rating created successfully!';
    } catch (error) {
        message.value = `Error: ${error.response?.data || error.message}`;
    }
};
</script>

<style scoped>
/* Add any custom styles here */
</style>