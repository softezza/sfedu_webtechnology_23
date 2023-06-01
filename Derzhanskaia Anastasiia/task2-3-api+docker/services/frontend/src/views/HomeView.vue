<template>
  <section>
    <div class="row justify-content-md-center">
    <div class="mx-auto col-md-auto text">This site help you achieve your goals!</div>
  </div>
    <img alt="Smart goal" src="../assets/smart-page.jpg" class="rounded mx-auto d-block">
    <div class="row justify-content-md-center">
    <div class="col col-md-auto mx-auto text" v-if="isLoggedIn" id="logout">
      <p id="logout">Click <a href="/dashboard">here</a> to view all goals.</p>
    </div>
    <div class="col col-md-auto mx-auto text" v-else>
      <span><a href="/register">Register</a></span>
      <span> or </span>
      <span><a href="/login">Log In</a></span>
    </div>
  </div>
    </section>
</template>

<script>
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'HomeView',
  computed : {
    isLoggedIn: function() {
      return this.$store.getters.isAuthenticated;
    }
  },
});
</script>

<style>
.text{
  font-size: 20px;
}
</style>