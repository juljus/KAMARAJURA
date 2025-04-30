<template>
    <div v-if="game && game.game_name" class="container p-10">
        <h1 class="text-4xl text-gray-300 mb-4 font-bold">{{ game.game_name }}</h1>
        <p class="text-gray-400 mb-6 italic">{{ game.game_description }}</p>

        <div class="mb-6 bg-stone-800 p-6 rounded-lg shadow-lg">
            <h2 class="text-2xl text-gray-300 font-semibold">Average Rating: {{ averageRating }}/10</h2>
        </div>

        <div class="bg-stone-900 p-6 rounded-lg shadow-lg">
            <h3 class="text-xl text-gray-300 mb-4 font-semibold">Reviews</h3>
            <ul v-if="reviews.length > 0">
                <li v-for="review in reviews" :key="review.rating_id" class="p-4 bg-stone-800 text-gray-300 rounded-lg mb-2 shadow-md">
                    <p><strong>Rating:</strong> {{ review.rating_score }}/10</p>
                    <p><strong>Comment:</strong> {{ review.rating_description }}</p>
                    <p><strong>User:</strong> {{ review.username }}</p>
                </li>
            </ul>
            <p v-else class="text-gray-400">No reviews</p>
        </div>

        <div v-if="userHasReviewed" class="bg-stone-900 p-6 rounded-lg shadow-lg mt-6">
            <h3 class="text-xl text-gray-300 mb-4 font-semibold">You have already reviewed this game</h3>
            <p class="text-gray-400">Thank you for your feedback!</p>
        </div>
        <div v-else-if="isLoggedIn" class="bg-stone-900 p-6 rounded-lg shadow-lg mt-6">
            <h3 class="text-xl text-gray-300 mb-4 font-semibold">Leave a Review</h3>
            <form @submit.prevent="submitReview">
                <div class="mb-4">
                    <label for="ratingScore" class="block text-gray-400">Rating (1-10)</label>
                    <input
                        type="number"
                        id="ratingScore"
                        v-model.number="reviewData.rating_score"
                        class="block w-full p-2 bg-stone-950 text-gray-400 border border-gray-400 rounded-lg"
                        placeholder="Enter your rating"
                        min="1"
                        max="10"
                        required
                    />
                </div>
                <div class="mb-4">
                    <label for="ratingDescription" class="block text-gray-400">Review</label>
                    <textarea
                        id="ratingDescription"
                        v-model="reviewData.rating_description"
                        class="block w-full p-2 bg-stone-950 text-gray-400 border border-gray-400 rounded-lg"
                        placeholder="Write your review"
                        rows="4"
                        required
                    ></textarea>
                </div>
                <button
                    type="submit"
                    class="text-gray-400 bg-sky-900 hover:bg-sky-950 focus:ring-blue-900 rounded-lg px-4 py-2"
                >
                    Submit Review
                </button>
            </form>
            <p v-if="reviewMessage" class="mt-4 text-gray-400">{{ reviewMessage }}</p>
        </div>
    </div>
    <div v-else class="text-center text-gray-300 mt-10">
        <p>Loading game details...</p>
    </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
import { useRoute } from 'vue-router';

// Define the token variable
const token = ref('');

if (process.client) {
    token.value = localStorage.getItem('authToken') || '';
}

const route = useRoute();
const gameId = route.params.id;

// Debugging: Log the gameId to ensure it is retrieved correctly
console.log('Route params:', route.params);

// Ensure gameId is retrieved correctly and add a fallback
if (!gameId) {
    console.error('Game ID is undefined. Check the route parameters.');
} else {
    console.log('Game ID retrieved successfully:', gameId);
}

const game = ref({
    game_name: 'Unknown Game',
    game_description: 'No description available.'
});

const reviews = ref([]);

const isLoading = ref(true);

const fetchGameDetails = async () => {
    try {
        const response = await axios.get(`http://localhost:5005/api/game/${gameId}`);
        console.log('API response for game details:', response.data);
        game.value = response.data;
    } catch (error) {
        console.error('Error fetching game details:', error);
        game.value = {
            game_name: 'Error Loading Game',
            game_description: 'Unable to load game details. Please try again later.'
        };
    } finally {
        isLoading.value = false;
    }
};

const fetchReviews = async () => {
    try {
        console.log('Fetching reviews for game:', gameId);
        const response = await axios.get(`http://localhost:5005/api/rating/game/${gameId}`);
        console.log('Raw reviews fetched:', response.data);
        reviews.value = response.data.map(review => ({
            rating_id: review.rating_id,
            rating_score: review.rating_score,
            rating_description: review.rating_description,
            username: review.username
        }));
        console.log('Processed reviews:', reviews.value);
    } catch (error) {
        console.error('Error fetching reviews:', error);
    }
};

const averageRating = computed(() => {
    if (reviews.value.length === 0) return 'N/A';
    const total = reviews.value.reduce((sum, review) => sum + review.rating_score, 0);
    return (total / reviews.value.length).toFixed(1);
});

const reviewData = ref({
    game_id: gameId,
    user_id: null, // Will be set from the token
    rating_score: null,
    rating_description: ''
});

const reviewMessage = ref('');

if (process.client && token.value) {
    const payload = JSON.parse(atob(token.value.split('.')[1]));
    reviewData.value.user_id = payload.nameid; // Extract user_id from the token
}

const isLoggedIn = computed(() => !!token.value);

const userHasReviewed = computed(() => {
    if (!isLoggedIn || !reviewData.value.user_id) return false;
    return reviews.value.some(review => review.user_id === reviewData.value.user_id);
});

const submitReview = async () => {
    try {
        const response = await axios.post('http://localhost:5005/api/rating', reviewData.value, {
            headers: {
                Authorization: `Bearer ${token.value}`
            }
        });
        reviewMessage.value = 'Review submitted successfully!';
        fetchReviews(); // Refresh the reviews
    } catch (error) {
        reviewMessage.value = error.response?.data || 'Failed to submit review. Please try again.';
    }
};

onMounted(() => {
    fetchGameDetails();
    fetchReviews();
});
</script>

<style scoped>
.container {
    max-width: 800px;
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

p {
    line-height: 1.6;
}

.bg-stone-800 {
    background-color: #2d2d2d;
}

.bg-stone-900 {
    background-color: #1f1f1f;
}

.shadow-lg {
    box-shadow: 0 10px 15px rgba(0, 0, 0, 0.2);
}

.shadow-md {
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}
</style>