<template>
    <section>
      <h1>Edit goal</h1>
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
  </template>
  
  <script>
  import { defineComponent } from 'vue';
  import { mapGetters, mapActions } from 'vuex';
  
  export default defineComponent({
    name: 'EditGoal',
    props: ['id'],
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
      this.GetGoal();
    },
    computed: {
      ...mapGetters({ goal: 'stateGoal' }),
    },
    methods: {
      ...mapActions(['updateGoal', 'viewGoal']),
      async submit() {
      try {
        let goal = {
          id: this.id,
          form: this.form,
        };
        await this.updateGoal(goal);
        this.$router.push({name: 'Goal', params:{id: this.goal.id}});
      } catch (error) {
        console.log(error);
      }
      },
      async GetGoal() {
        try {
          await this.viewGoal(this.id);
          this.form.title = this.goal.title;
          this.form.specific_part = this.goal.specific_part;
          this.form.measureable_part = this.goal.measureable_part;
          this.form.attainable_part = this.goal.attainable_part;
          this.form.relevant_part = this.goal.relevant_part;
          this.form.due_time = this.goal.due_time;
        } catch (error) {
          console.error(error);
          this.$router.push('/dashboard');
        }
      }
    },
  });
  </script>