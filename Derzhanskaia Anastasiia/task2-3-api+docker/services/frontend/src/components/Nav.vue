<template>
  <header>
    <nav class="navbar navbar-expand-md navbar-dark">
      <div class="container">
        <a class="navbar-brand" href="/">
          <img alt="Logo" src="../assets/goal.png" class="mt-0 mb-0">
        </a>
        <h1 class="mt-2"> Smart Goal setter</h1>
        <div class=" float-right " id="navbarCollapse">
          <ul v-if="isLoggedIn" class="navbar-nav mr-0 pr-0">
            <li class="nav-item">
              <router-link class="nav-link" to="/">Home</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/dashboard">Dashboard</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/profile">Profile</router-link>
            </li>
            <li class="nav-item">
              <a class="nav-link" @click="logout">Log Out</a>
            </li>
          </ul>
          <ul v-else class="navbar-nav  mr-0 pr-0">
            <li class="nav-item">
              <router-link class="nav-link" to="/register">Register</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/login">Log In</router-link>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </header>
</template>

<script>
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'NavBar',
  computed: {
    isLoggedIn: function() {
      return this.$store.getters.isAuthenticated;
    }
  },
  methods: {
    async logout () {
      await this.$store.dispatch('logOut');
      this.$router.push('/login');
    }
  },
});
</script>

<style scoped>
a {
  cursor: pointer;
}
h1{
  color: white;
}
.nav-link{
  font-size: 18px;
}
nav{
  background: #000;
}

</style>