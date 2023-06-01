<template>
    <footer>
      <div class="footer navbar navbar-default">
        <div class="copyright col-md-2 mx-auto">&copy; All rights reserved</div>
      </div>
    </footer>
  </template>
  
  <script>
  import { defineComponent } from 'vue';
  
  export default defineComponent({
    name: 'FooterVue',
    },
  );
  </script>
 
  <style scoped>
 footer{
    background: #000;
    height: 5.5em;
    margin-top: auto;
 }
.copyright
{
    font-size: 1.5em;
    color:white;
}
  </style>