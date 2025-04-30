<template>
    <div class="container p-10">
        <div class="grid gap-6 mb-6 md:grid-cols-2">
            <div class="bg-stone-900 p-6 rounded-lg shadow-lg">
                <h2 class="text-2xl text-gray-300 mb-4 font-semibold">Login</h2>
                <form @submit.prevent="login">
                    <div class="mb-4">
                        <label for="loginUsername" class="block text-gray-400">Username</label>
                        <input
                            type="text"
                            id="loginUsername"
                            v-model="loginData.user_name"
                            class="block w-full p-2 bg-stone-950 text-gray-400 border border-gray-400 rounded-lg"
                            placeholder="Enter your username"
                            required
                        />
                    </div>
                    <div class="mb-4 relative">
                        <label for="loginPassword" class="block text-gray-400">Password</label>
                        <input
                            :type="showLoginPassword ? 'text' : 'password'"
                            id="loginPassword"
                            v-model="loginData.user_password"
                            class="block w-full p-2 bg-stone-950 text-gray-400 border border-gray-400 rounded-lg"
                            placeholder="Enter your password"
                            required
                        />
                        <span
                            class="absolute inset-y-0 right-3 flex items-center cursor-pointer text-gray-400"
                            @click="toggleLoginPasswordVisibility"
                        >
                            <i :class="showLoginPassword ? 'fa fa-eye-slash' : 'fa fa-eye'"></i>
                        </span>
                    </div>
                    <button
                        type="submit"
                        class="text-gray-400 bg-sky-900 hover:bg-sky-950 focus:ring-blue-900 rounded-lg px-4 py-2"
                    >
                        Login
                    </button>
                </form>
                <p v-if="loginMessage" class="mt-4 text-gray-400">{{ loginMessage }}</p>
            </div>

            <div class="bg-stone-900 p-6 rounded-lg shadow-lg">
                <h2 class="text-2xl text-gray-300 mb-4 font-semibold">Create Account</h2>
                <form @submit.prevent="createAccount">
                    <div class="mb-4">
                        <label for="signupUsername" class="block text-gray-400">Username</label>
                        <input
                            type="text"
                            id="signupUsername"
                            v-model="signupData.user_name"
                            class="block w-full p-2 bg-stone-950 text-gray-400 border border-gray-400 rounded-lg"
                            placeholder="Choose a username"
                            required
                        />
                    </div>
                    <div class="mb-4 relative">
                        <label for="signupPassword" class="block text-gray-400">Password</label>
                        <input
                            :type="showSignupPassword ? 'text' : 'password'"
                            id="signupPassword"
                            v-model="signupData.user_password"
                            class="block w-full p-2 bg-stone-950 text-gray-400 border border-gray-400 rounded-lg"
                            placeholder="Choose a password"
                            required
                        />
                        <span
                            class="absolute inset-y-0 right-3 flex items-center cursor-pointer text-gray-400"
                            @click="toggleSignupPasswordVisibility"
                        >
                            <i :class="showSignupPassword ? 'fa fa-eye-slash' : 'fa fa-eye'"></i>
                        </span>
                    </div>
                    <button
                        type="submit"
                        class="text-gray-400 bg-sky-900 hover:bg-sky-950 focus:ring-blue-900 rounded-lg px-4 py-2"
                    >
                        Create Account
                    </button>
                </form>
                <p v-if="signupMessage" class="mt-4 text-gray-400">{{ signupMessage }}</p>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import axios from 'axios';

const loginData = ref({
    user_name: '',
    user_password: ''
});

const signupData = ref({
    user_name: '',
    user_password: ''
});

const showLoginPassword = ref(false);
const showSignupPassword = ref(false);

const loginMessage = ref('');
const signupMessage = ref('');

const toggleLoginPasswordVisibility = () => {
    showLoginPassword.value = !showLoginPassword.value;
};

const toggleSignupPasswordVisibility = () => {
    showSignupPassword.value = !showSignupPassword.value;
};

const login = async () => {
    try {
        const response = await axios.post('http://localhost:5005/api/user/login', loginData.value);
        loginMessage.value = 'Login successful!';
    } catch (error) {
        loginMessage.value = error.response?.data || 'Login failed. Please check your credentials.';
    }
};

const createAccount = async () => {
    try {
        const response = await axios.post('http://localhost:5005/api/user', signupData.value);
        signupMessage.value = 'Account created successfully!';
        signupData.value = { user_name: '', user_password: '' };
    } catch (error) {
        signupMessage.value = error.response?.data || 'Account creation failed. Please try again.';
    }
};
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

h2 {
    color: #e2e8f0;
    text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5);
}

p {
    line-height: 1.6;
}

.bg-stone-900 {
    background-color: #1f1f1f;
}

.shadow-lg {
    box-shadow: 0 10px 15px rgba(0, 0, 0, 0.2);
}

button {
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
    box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.2);
}
button:active {
    transform: translateY(2px);
    box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.2);
}

/* Add styles for the eye icon */
.fa {
    font-size: 1.2rem;
}
</style>