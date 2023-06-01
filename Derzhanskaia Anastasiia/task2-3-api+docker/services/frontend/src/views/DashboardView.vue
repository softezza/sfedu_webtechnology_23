<template>
    <div>
      <section>
        <h1>Add new goal</h1>
        <hr/><br/>
  
        <form @submit.prevent="submit">
          <div class="mb-3">
            <label for="title" class="form-label">Title:</label>
            <input type="text" name="title" v-model="form.title" class="form-control" />
          </div>
          <div class="mb-3">
          <label for="specific" class="form-label">Specific:</label>
          <textarea
            name="specific"
            v-model="form.specific_part"
            class="form-control"
          ></textarea>
        </div>
        <div class="mb-3">
          <label for="measureable" class="form-label">Measureable:</label>
          <textarea
            name="measureable"
            v-model="form.measureable_part"
            class="form-control"
          ></textarea>
        </div>
        <div class="mb-3">
          <label for="attainable" class="form-label">Attainable:</label>
          <textarea
            name="attainable"
            v-model="form.attainable_part"
            class="form-control"
          ></textarea>
        </div>
        <div class="mb-3">
          <label for="relevant" class="form-label">Relevant:</label>
          <textarea
            name="relevant"
            v-model="form.relevant_part"
            class="form-control"
          ></textarea>
        </div>
        <div class="mb-3">
          <label for="due_time" class="form-label">Time:</label>
          <textarea
            name="due_time"
            v-model="form.due_time"
            class="form-control"
          ></textarea>
        </div>
          <button type="submit" class="btn btn-primary">Submit</button>
        </form>
      </section>
  
      <br/><br/>
  
      <section>
        <h1>Goals</h1>
        <hr/><br/>
  
        <div v-if="goals.length">
          <div v-for="goal in goals" :key="goal.id" class="goals">
            <div class="card" style="width: 18rem;">
              <div class="card-body">
                <ul>
                  <li><strong>Goal Title:</strong> {{ goal.title }}</li>
                  <li><router-link :to="{name: 'Goal', params:{id: goal.id}}">View</router-link></li>
                </ul>
              </div>
            </div>
            <br/>
          </div>
        </div>
  
        <div v-else>
          <p>Nothing to see. Check back later.</p>
        </div>
      </section>
    </div>
  </template>
  
  <script>
  import { defineComponent } from 'vue';
  import { mapGetters, mapActions } from 'vuex';
  
  export default defineComponent({
    name: 'DashboardView',
    data() {
      return {
        form: {
          title: '',
          specific_part: '',
          measureable_part: '', 
          attainable_part: '', 
          relevant_part: '', 
          due_time: ''
        },
      };
    },
    created: function() {
      return this.$store.dispatch('getGoals');
    },
    computed: {
      ...mapGetters({ goals: 'stateGoals'}),
    },
    methods: {
      ...mapActions(['createGoal']),
      async submit() {
        await this.createGoal(this.form);
      },
    },
  });
  </script>