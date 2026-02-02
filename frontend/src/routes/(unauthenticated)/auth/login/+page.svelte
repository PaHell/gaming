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

	let { form } = $props();
	let pending = $state(false);
	let returnUrl = $state(
		page.url.searchParams.has('returnUrl')
			? '?returnUrl=' + encodeURIComponent(page.url.searchParams.get('returnUrl') ?? '')
			: ''
	);
</script>

<Card.Header class="text-center">
	<Card.Title class="text-xl">Welcome back</Card.Title>
	<Card.Description>Sign in to your account</Card.Description>
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
				<Button type="submit" disabled={pending} class="w-full">Sign in</Button>

				<FieldDescription class="text-center">
					Don’t have an account?
					<a href="/auth/register{returnUrl}" class="underline">Create one</a>
				</FieldDescription>
			</Field>
		</FieldGroup>
	</form>
</Card.Content>
