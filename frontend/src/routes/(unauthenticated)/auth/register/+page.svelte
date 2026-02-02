<script lang="ts">
	import { enhance } from '$app/forms';
	import { page } from '$app/state';
	import { Button } from '$lib/shadcn/components/ui/button/index.js';
	import * as Card from '$lib/shadcn/components/ui/card/index.js';
	import {
		FieldGroup,
		Field,
		FieldLabel,
		FieldDescription
	} from '$lib/shadcn/components/ui/field/index.js';
	import { Input } from '$lib/shadcn/components/ui/input/index.js';

	let { form, data } = $props();
	let pending = $state(false);
	let returnUrl = $state(
		page.url.searchParams.has('returnUrl')
			? '?returnUrl=' + encodeURIComponent(page.url.searchParams.get('returnUrl') ?? '')
			: ''
	);
</script>

<Card.Header class="text-center">
	<Card.Title class="text-xl">Greetings!</Card.Title>
	<Card.Description>Please sign up</Card.Description>
</Card.Header>
<Card.Content>
	<form
		method="POST"
		use:enhance={() => {
			pending = true;
			return async () => {
				pending = false;
			};
		}}
	>
		<FieldGroup>
			{#if form?.error}
				<div
					class="rounded-md border border-destructive bg-destructive/10 p-3 text-sm text-destructive"
				>
					{form.error}
				</div>
			{/if}
			<div class="grid grid-cols-1 gap-4 gap-y-8 md:grid-cols-2">
				<Field>
					<FieldLabel for="first-name">First Name</FieldLabel>
					<Input
						id="first-name"
						name="firstName"
						type="fname"
						placeholder="Yannis"
						value={form?.values?.firstName}
						required
					/>
				</Field>
				<Field>
					<FieldLabel for="last-name">Last Name</FieldLabel>
					<Input
						id="last-name"
						name="lastName"
						type="lname"
						placeholder="from Accounting"
						value={form?.values?.lastName}
						required
					/>
				</Field>
			</div>
			<Field>
				<FieldLabel for="email">Email</FieldLabel>
				<Input
					id="email"
					name="email"
					type="email"
					placeholder="m@example.com"
					value={form?.values?.email}
					required
				/>
			</Field>
			<Field>
				<FieldLabel for="password">Password</FieldLabel>
				<Input
					id="password"
					name="password"
					type="password"
					value={form?.values?.password}
					required
				/>
			</Field>
			<Field>
				<Button disabled={pending} type="submit">Register</Button>
				<FieldDescription class="text-center">
					Already have an account? <a href="/auth/login{returnUrl}">Sign in</a>
				</FieldDescription>
			</Field>
		</FieldGroup>
	</form>
</Card.Content>
