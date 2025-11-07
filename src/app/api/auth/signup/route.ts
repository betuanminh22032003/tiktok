// src/app/api/auth/signup/route.ts
import { NextResponse } from 'next/server';
import bcrypt from 'bcryptjs';
import { users } from '@/lib/db';
import { User } from '@/lib/types';

export async function POST(request: Request) {
  try {
    const { email, password, username } = await request.json();

    if (!email || !password || !username) {
      return NextResponse.json({ message: 'Missing fields' }, { status: 400 });
    }

    const existingUser = users.find(
      (user) => user.email === email || user.username === username
    );

    if (existingUser) {
      return NextResponse.json({ message: 'User already exists' }, { status: 409 });
    }

    const passwordHash = await bcrypt.hash(password, 10);

    const newUser: User = {
      id: String(users.length + 1),
      email,
      username,
      passwordHash,
    };

    users.push(newUser);

    console.log('New user created:', { email, username });
    console.log('Current users:', users);

    return NextResponse.json({ message: 'User created successfully' }, { status: 201 });
  } catch (error) {
    console.error('Signup error:', error);
    return NextResponse.json({ message: 'An error occurred' }, { status: 500 });
  }
}
