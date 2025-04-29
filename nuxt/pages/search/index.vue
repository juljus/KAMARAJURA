<template>
    <div class="container p-10">
        <div class="relative">
            <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
                <svg class="w-4 h-4 text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                    <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"/>
                </svg>
            </div>
            <input type="search" id="search" class="block w-full p-4 ps-10 bg-stone-950 text-gray-400 border border-gray-400 font-mono rounded-lg focus:ring-blue-900 focus:border-blue-900" placeholder="Da game finder huzzaa" required />
            <button type="submit" class="text-gray-400 absolute end-2.5 bottom-2.5 bg-sky-900 hover:bg-sky-950 focus:ring-blue-900 rounded-lg font-mono border-b-2 w-full sm:w-auto px-4 py-2">Search :P</button>
        </div>
    </div>
</template>

<script setup>

</script>

<style scoped>

</style>