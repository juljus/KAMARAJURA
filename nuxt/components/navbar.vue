<template>
    <div>
        <nav class="p-6 mb-6 border-b border-gray-400 bg-stone-900">
            <ul class="flex justify-between items-center">
                <li class="px-4 py-2 text-center transition-transform duration-200 ease-in-out hover:scale-105 hover:text-gray-300"> 
                    <NuxtLink to="/" class="block">KAMARAJURA</NuxtLink> 
                </li>
                <li class="px-4 py-2 text-center transition-transform duration-200 ease-in-out hover:scale-105 hover:text-gray-300"> 
                    <NuxtLink to="/game/create" class="block">CREATE GAME</NuxtLink>
                </li>
                <li class="px-4 py-2 text-center transition-transform duration-200 ease-in-out hover:scale-105 hover:text-gray-300">
                    <NuxtLink to="/auth" class="block">
                        <span v-if="isLoggedIn">{{ username }}</span>
                        <span v-else>Login</span>
                    </NuxtLink>
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
nav {
    background-color: #1f1f1f;
    border-radius: 6px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

ul {
    display: flex;
    gap: 1rem;
}

li {
    color: #e2e8f0;
    font-weight: bold;
    cursor: pointer;
}

li:hover {
    text-shadow: 0px 0px 5px rgba(255, 255, 255, 0.5);
}
</style>