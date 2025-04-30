<template>
    <div class="container p-10">
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
                </li>
            </ul>
            <p v-else class="text-gray-400">No reviews</p>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
import { useRoute } from 'vue-router';

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

const game = ref({});
const reviews = ref([]);

const fetchGameDetails = async () => {
    try {
        const response = await axios.get(`http://localhost:5005/api/game/${gameId}`);
        game.value = response.data;
    } catch (error) {
        console.error('Error fetching game details:', error);
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
            rating_description: review.rating_description
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