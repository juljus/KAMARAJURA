<template>
    <div>
        <nav class="p-10 mb-8 border-b-2 border-gray-400">
            <ul class="grid-cols-3 grid">
                <li class="p-4 text-center transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 hover:text-gray-300"> <NuxtLink to="/">KAMARAJURA</NuxtLink> </li>
                <li class="p-4 text-center transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 hover:text-gray-300"> <NuxtLink to="/search/">FIND GAMES</NuxtLink></li>
                <li class="p-4 text-center transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 hover:text-gray-300">
                    <div class="flex items-center">
                        <a href="/auth" class="text-gray-300 hover:text-gray-400">
                            <span v-if="isLoggedIn">{{ username }}</span>
                            <span v-else>Login</span>
                        </a>
                    </div>
                </li>
            </ul>
        </nav>
    </div>
</template>

<script setup>
const token = ref('');
const username = ref('');

if (process.client) {
    token.value = localStorage.getItem('authToken') || '';
    if (token.value) {
        // Decode the token to extract the username (basic decoding for demonstration purposes)
        const payload = JSON.parse(atob(token.value.split('.')[1]));
        username.value = payload.unique_name || payload.name || 'User';
    }
}

const isLoggedIn = computed(() => !!token.value);
</script>

<style scoped>
</style>