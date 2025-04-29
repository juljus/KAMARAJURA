<template>
    <div class="grid gap-6 mb-6 md:grid-cols-2">
        <div>
            <label for="Username" class="block mb-2 text-gray-400 bg-stone-900 font-mono">Username</label>
            <input type="text" id="user_name" class="bg-stone-950 border-b-2 border-gray-400 text-gray-400 font-mono rounded-lg focus:ring-blue-900 focus:border-blue-900 block w-full p-2.5" placeholder="User HERE" required />"
        </div>
        <div>
            <label for="Password" class="block mb-2 text-gray-400 bg-stone-900 font-mono">Password</label>
            <input type="Password" id="user_password" class="bg-stone-950 border-b-2 border-gray-400 text-gray-400 font-mono rounded-lg focus:ring-blue-900 focus:border-blue-900 block w-full p-2.5" placeholder="Pass HERE" required />"
        </div>
        <button type="submit" class="text-gray-400 bg-sky-900 hover:bg-sky-950 focus:ring-blue-900 rounded-lg font-mono border-b-2 w-full sm:w-auto px-5 py-2.5 text-center justify-self-center">Submit :P</button>
    </div>
</template>

<script setup>

</script>

<style scoped>

</style>