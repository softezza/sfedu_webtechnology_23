<template>
    <div v-if="goal">
      <p><strong>Title:</strong> {{ goal.title }}</p>
      <p><strong>Content:</strong> {{ goal.content }}</p>
      <p><strong>Specific</strong>{{ goal.specific_part }}</p>     
      <p><strong>Measureable</strong>{{ goal.measureable_part }}</p>
      <p><strong>Attainable</strong>{{ goal.attainable_part }}</p>
      <p><strong>Relevant</strong>{{ goal.relevant_part }}</p>
      <p><strong>Due time</strong>{{ goal.due_time }}</p>
      <!-- <p><strong>Author:</strong> {{ goal.author.username }}</p> -->
  
      <div v-if="user.id === goal.author.id">
        <p><router-link :to="{name: 'EditGoal', params:{id: goal.id}}" class="btn btn-primary">Edit</router-link></p>
        <p><button @click="removeGoal()" class="btn btn-secondary">Delete</button></p>
      </div>
    </div>
  </template>
  
  
  <script>
  import { defineComponent } from 'vue';
  import { mapGetters, mapActions } from 'vuex';
  
  export default defineComponent({
    name: 'GoalView',
    props: ['id'],
    async created() {
      try {
        await this.viewGoal(this.id);
      } catch (error) {
        console.error(error);
        this.$router.push('/dashboard');
      }
    },
    computed: {
      ...mapGetters({ goal: 'stateGoal', user: 'stateUser'}),
    },
    methods: {
      ...mapActions(['viewGoal', 'deleteGoal']),
      async removeGoal() {
        try {
          await this.deleteGoal(this.id);
          this.$router.push('/dashboard');
        } catch (error) {
          console.error(error);
        }
      }
    },
  });
  </script>