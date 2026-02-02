<script lang="ts">
	import { Button } from "$lib/shadcn/components/ui/button/index.js";
	import * as Card from "$lib/shadcn/components/ui/card/index.js";
	import {
		FieldGroup,
		Field,
		FieldLabel,
		FieldDescription,
	} from "$lib/shadcn/components/ui/field/index.js";
	import { Input } from "$lib/shadcn/components/ui/input/index.js";
	import { cn } from "$lib/shadcn/utils.js";
	import type { HTMLAttributes } from "svelte/elements";
	import { AuthenticationClient } from "$lib/clients.js";
	import { env } from '$env/dynamic/public';
	import { goto } from '$app/navigation';
	import { toast } from 'svelte-sonner';

	let { class: className, ...restProps }: HTMLAttributes<HTMLDivElement> = $props();

	const id = $props.id();
	
	let firstName = $state('');
	let lastName = $state('');
	let email = $state('');
	let password = $state('');
	let loading = $state(false);

	const authClient = new AuthenticationClient(env.PUBLIC_BACKEND_URL_CLIENT);

	async function handleSubmit(event: Event) {
		event.preventDefault();
		loading = true;

		console.log('Registration attempt:', { email, firstName, lastName });

		try {
			const session = await authClient.register({ 
				email, 
				password, 
				firstName, 
				lastName 
			});
			console.log('Registration successful:', { sessionId: session.id, userId: session.userId });
			toast.success('Registration successful! Redirecting...');
			goto('/app/dashboard');
		} catch (error: any) {
			console.error('Registration failed:', error);
			if (error?.status === 401) {
				toast.error('Registration failed. Email may already be in use.');
			} else {
				toast.error('Registration failed. Please try again.');
			}
		} finally {
			loading = false;
		}
	}
</script>

<div class={cn("flex flex-col gap-6", className)} {...restProps}>
	<Card.Root>
		<Card.Header class="text-center">
			<Card.Title class="text-xl">Create an account</Card.Title>
			<Card.Description>Enter your information to get started</Card.Description>
		</Card.Header>
		<Card.Content>
			<form onsubmit={handleSubmit}>
				<FieldGroup>
					<Field>
						<FieldLabel for="firstName-{id}">First Name</FieldLabel>
						<Input id="firstName-{id}" type="text" placeholder="John" bind:value={firstName} required />
					</Field>
					<Field>
						<FieldLabel for="lastName-{id}">Last Name</FieldLabel>
						<Input id="lastName-{id}" type="text" placeholder="Doe" bind:value={lastName} required />
					</Field>
					<Field>
						<FieldLabel for="email-{id}">Email</FieldLabel>
						<Input id="email-{id}" type="email" placeholder="m@example.com" bind:value={email} required />
					</Field>
					<Field>
						<FieldLabel for="password-{id}">Password</FieldLabel>
						<Input id="password-{id}" type="password" bind:value={password} required />
					</Field>
					<Field>
						<Button type="submit" disabled={loading}>
							{loading ? 'Creating account...' : 'Create account'}
						</Button>
						<FieldDescription class="text-center">
							Already have an account? <a href="/auth/login">Sign in</a>
						</FieldDescription>
					</Field>
				</FieldGroup>
			</form>
		</Card.Content>
	</Card.Root>
	<FieldDescription class="px-6 text-center">
		By clicking continue, you agree to our <a href="##">Terms of Service</a>
		and <a href="##">Privacy Policy</a>.
	</FieldDescription>
</div>
