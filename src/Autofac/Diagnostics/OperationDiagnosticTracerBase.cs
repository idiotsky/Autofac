﻿// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Diagnostics;

/// <summary>
/// Base class for tracers that require all operations for logical operation tracing.
/// </summary>
/// <typeparam name="TContent">
/// The type of content generated by the trace at the end of the operation.
/// </typeparam>
/// <remarks>
/// <para>
/// Derived classes will be subscribed to all Autofac diagnostic events
/// and will raise an <see cref="OperationDiagnosticTracerBase{TContent}.OperationCompleted"/>
/// event when a logical operation has finished and trace data is available.
/// </para>
/// </remarks>
public abstract class OperationDiagnosticTracerBase<TContent> : DiagnosticTracerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OperationDiagnosticTracerBase{TContent}"/> class
    /// and enables all subscriptions.
    /// </summary>
    protected OperationDiagnosticTracerBase()
    {
        EnableAll();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OperationDiagnosticTracerBase{TContent}"/> class
    /// and enables a specified set of subscriptions.
    /// </summary>
    /// <param name="subscriptions">
    /// The set of subscriptions that should be enabled by default.
    /// </param>
    protected OperationDiagnosticTracerBase(IEnumerable<string> subscriptions)
    {
        if (subscriptions == null)
        {
            throw new ArgumentNullException(nameof(subscriptions));
        }

        foreach (var subscription in subscriptions)
        {
            EnableBase(subscription);
        }
    }

    /// <inheritdoc/>
    public override void Enable(string diagnosticName)
    {
        throw new NotSupportedException(TracerMessages.SubscriptionsDisabled);
    }

    /// <inheritdoc/>
    public override void Disable(string diagnosticName)
    {
        throw new NotSupportedException(TracerMessages.SubscriptionsDisabled);
    }

    /// <summary>
    /// Event raised when a resolve operation completes and trace data is available.
    /// </summary>
    [SuppressMessage("CA1003", "CA1003", Justification = "Breaking API change.")]
    public event EventHandler<OperationTraceCompletedArgs<TContent>>? OperationCompleted;

    /// <summary>
    /// Gets the number of operations in progress being traced.
    /// </summary>
    /// <value>
    /// An <see cref="int"/> with the number of trace IDs associated
    /// with in-progress operations being traced by this tracer.
    /// </value>
    public abstract int OperationsInProgress { get; }

    /// <summary>
    /// Invokes the <see cref="OperationCompleted"/> event.
    /// </summary>
    /// <param name="args">
    /// The arguments to provide in the raised event.
    /// </param>
    protected virtual void OnOperationCompleted(OperationTraceCompletedArgs<TContent> args)
    {
        OperationCompleted?.Invoke(this, args);
    }
}