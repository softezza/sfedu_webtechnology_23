import axios from 'axios';

const state = {
  goals: null,
  goal: null
};

const getters = {
  stateGoals: state => state.goals,
  stateGoal: state => state.goal,
};

const actions = {
  async createGoal({dispatch}, goal) {
    await axios.post('goals', goal);
    await dispatch('getGoals');
  },
  async getGoals({commit}) {
    let {data} = await axios.get('goals');
    commit('setGoals', data);
  },
  async viewGoal({commit}, id) {
    let {data} = await axios.get(`goal/${id}`);
    commit('setGoal', data);
  },
  // eslint-disable-next-line no-empty-pattern
  async updateGoal({}, goal) {
    await axios.patch(`goal/${goal.id}`, goal.form);
  },
  // eslint-disable-next-line no-empty-pattern
  async deleteGoal({}, id) {
    await axios.delete(`goal/${id}`);
  }
};

const mutations = {
  setGoals(state, goals){
    state.goals = goals;
  },
  setGoal(state, goal){
    state.goal = goal;
  },
};

export default {
  state,
  getters,
  actions,
  mutations
};