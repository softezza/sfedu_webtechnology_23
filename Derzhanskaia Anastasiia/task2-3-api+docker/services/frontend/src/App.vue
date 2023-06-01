<template>
  <div id="app">
    <NavBar />
    <div class="main container">
      <router-view/>
    </div>
    <FooterVue/>
  </div>
  
</template>

<script>
// @ is an alias to /src
import NavBar from '@/components/Nav.vue'
import FooterVue from '@/components/Footer.vue'
export default {
  components: {
    NavBar,
    FooterVue
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}
.main {
  padding-top: 5em;
}

</style>