# Pandemizer Documentation

- [Basics](#basics)
  - [Pop](#pop)
  - [Iteration](#iteration)
  - [Pop Index](#pop-index)
- [Characteristic Features](#characteristic-features)
  - [StateOfLive](#stateoflive)
  - [Age](#age)
- [Simulation Settings](#simulation-settings)
- [Simulation Systems](#simulation-systems)

---

## Basics

### Pop
> A Pop represents and encompasses a population group. This group is composed of various characteristic features of the group.

### Iteration
> An iteration represents one step in the simulation. This is equivalent to a period of one week in the real world.
> Each iteration contains a single [Pop Index](#pop-index).

### Pop Index
> The PopIndex contains all the different [pops](#pop) that exist for the current [iteration](#iteration).

## Characteristic Features

---

### StateOfLive
> The StateOfLive describes the current state of health of a [pop](#pop). 
> This attribute separates into the following states:
> - Healthy: The pop is in good physical condition.
> - ImperceptiblyInfected: The pop is sick, but shows no symptoms. The disease is not noticeable.
> - Infected: The pop is sick. He shows noticeable symptoms.
> - HeavilyInfected: The pop is seriously ill. He shows very noticeable symptoms and is more vulnerable.
> - Dead: The Pop is dead. He is no longer simulated in the simulation.
> - Immune: The pop is immune. The pop is temporarily protected from the disease.

### Age
> 
---

## Simulation Settings
coming soon..

---

## Simulation Systems
coming soon..